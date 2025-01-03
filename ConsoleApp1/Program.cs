﻿
using HtmlAgilityPack;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Canvas.Parser;
using MasterKinder.Data;
using MasterKinder.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace PdfDataExtractor
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();
            await host.RunAsync();
        }

        static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureServices((context, services) =>
                {
                    services.AddDbContext<MrDb>(options =>
                        options.UseSqlServer(context.Configuration.GetConnectionString("DefaultSQLConnection")));

                    services.AddHostedService<PdfProcessor>();
                });
    }

    public class PdfProcessor : IHostedService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly IConfiguration _configuration;

        public PdfProcessor(IServiceProvider serviceProvider, IConfiguration configuration)
        {
            _serviceProvider = serviceProvider;
            _configuration = configuration;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            Console.WriteLine("StartAsync method has been called.");

            string url = "https://ssan.stockholm.se/anonym/webdokument/Delade%20dokument/Forms/AllItems.aspx?RootFolder=%2Fanonym%2Fwebdokument%2FDelade%20dokument%2FF%C3%B6rskolor%2F2024%2FS%C3%B6dermalm&FolderCTID=0x01200015B00A3B7947284E8A98F455403CF440&View=%7BCEB0BF65%2D2CB1%2D4A7B%2DA2B3%2DD82EE112AAA7%7D";
            string downloadDirectory = Path.Combine(Directory.GetCurrentDirectory(), "pdfs");
            Directory.CreateDirectory(downloadDirectory);

            List<string> pdfUrls = await GetPdfUrls(url);

            foreach (var pdfUrl in pdfUrls)
            {
                try
                {
                    Console.WriteLine($"Laddar ner {pdfUrl}");
                    string pdfPath = Path.Combine(downloadDirectory, Path.GetFileName(pdfUrl));
                    await DownloadPdf(pdfUrl, pdfPath);
                    Malibu malibu = ExtractDataFromPdf(pdfPath);

                    using (var scope = _serviceProvider.CreateScope())
                    {
                        var dbContext = scope.ServiceProvider.GetRequiredService<MrDb>();
                        dbContext.Malibus.Add(malibu);
                        await dbContext.SaveChangesAsync();
                    }

                    Console.WriteLine($"Namn: {malibu.Namn}, Helhetsomdome: {malibu.Helhetsomdome}, Svarsfrekvens: {malibu.Svarsfrekvens}, AntalSvar: {malibu.AntalSvar}, NormalizedNamn: {malibu.NormalizedNamn}");
                    foreach (var question in malibu.Questions)
                    {
                        Console.WriteLine($"  FrageText: {question.FrageText}, AndelInstammer: {question.AndelInstammer}, Year: {question.Year}");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Fel vid bearbetning av {pdfUrl}: {ex.Message}");
                }
            }

            // Håll konsolen öppen för att se utdata
            Console.WriteLine("Tryck på valfri tangent för att avsluta...");
            Console.ReadKey();
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            Console.WriteLine("StopAsync method has been called.");
            return Task.CompletedTask;
        }

        static async Task<List<string>> GetPdfUrls(string url)
        {
            HttpClient client = new HttpClient();
            string pageContent = await client.GetStringAsync(url);
            HtmlDocument document = new HtmlDocument();
            document.LoadHtml(pageContent);

            List<string> pdfUrls = new List<string>();
            foreach (HtmlNode link in document.DocumentNode.SelectNodes("//a[@href]"))
            {
                string href = link.GetAttributeValue("href", string.Empty);
                if (href.EndsWith(".pdf"))
                {
                    string fullUrl = new Uri(new Uri(url), href).ToString();
                    pdfUrls.Add(fullUrl);
                    Console.WriteLine($"Hittad PDF-länk: {fullUrl}");
                }
            }

            return pdfUrls;
        }

        static async Task DownloadPdf(string pdfUrl, string outputPath)
        {
            HttpClient client = new HttpClient();
            byte[] pdfBytes = await client.GetByteArrayAsync(pdfUrl);
            File.WriteAllBytes(outputPath, pdfBytes);
            Console.WriteLine($"PDF sparad: {outputPath}");
        }

        static Malibu ExtractDataFromPdf(string pdfPath)
        {
            PdfDocument pdfDoc = new PdfDocument(new PdfReader(pdfPath));
            string text = "";
            for (int i = 1; i <= pdfDoc.GetNumberOfPages(); i++)
            {
                text += PdfTextExtractor.GetTextFromPage(pdfDoc.GetPage(i));
            }
            Console.WriteLine($"Extraherad text från {pdfPath}:\n{text}");

            var lines = text.Split('\n').Select(line => line.Trim()).Where(line => !string.IsNullOrEmpty(line)).ToList();
            var malibu = new Malibu
            {
                Questions = new List<Question>()
            };

            // Extracting the name
            var nameIndex = lines.FindIndex(line => line.Contains("Vårdnadshavare Förskola") || line.Contains("Vårdnadshavare Familjedaghem"));
            if (nameIndex != -1 && nameIndex + 1 < lines.Count)
            {
                malibu.Namn = lines[nameIndex + 1].Trim();
                malibu.NormalizedNamn = malibu.Namn.ToLower().Replace(" ", "").Replace(";", "").Replace(";;", "");
                Console.WriteLine($"Extracted name: {malibu.Namn}");
            }

            if (string.IsNullOrWhiteSpace(malibu.Namn))
            {
                Console.WriteLine("Failed to extract name. PDF text:");
                foreach (var line in lines)
                {
                    Console.WriteLine(line);
                }
                throw new InvalidOperationException("Namn cannot be null or empty.");
            }

            // Extract overall rating (Helhetsomdome)
            var helhetsomdomeLineIndex = lines.FindIndex(line => line.Contains("HELHETSOMDÖME"));
            if (helhetsomdomeLineIndex != -1)
            {
                malibu.Helhetsomdome = lines[helhetsomdomeLineIndex + 1].Trim();
                Console.WriteLine($"Extracted helhetsomdome: {malibu.Helhetsomdome}");
            }
            else
            {
                Console.WriteLine("Failed to extract helhetsomdome. PDF text:");
                foreach (var line in lines)
                {
                    Console.WriteLine(line);
                }
                throw new InvalidOperationException("Helhetsomdome cannot be null or empty.");
            }

            // Extract response rate and number of responses
            var svarsfrekvensLineIndex = lines.FindIndex(line => line.Contains("svar") && line.Contains("%"));
            if (svarsfrekvensLineIndex != -1)
            {
                var svarsfrekvensLine = lines[svarsfrekvensLineIndex];
                var delar = svarsfrekvensLine.Split(',');
                if (delar.Length > 1)
                {
                    malibu.Svarsfrekvens = int.Parse(delar[1].Replace("%", "").Trim());
                    malibu.AntalSvar = int.Parse(delar[0].Split(' ')[0].Trim());
                    Console.WriteLine($"Extracted svarsfrekvens: {malibu.Svarsfrekvens}, antal svar: {malibu.AntalSvar}");
                }
            }
            else
            {
                Console.WriteLine("Failed to extract svarsfrekvens. PDF text:");
                foreach (var line in lines)
                {
                    Console.WriteLine(line);
                }
                throw new InvalidOperationException("Svarsfrekvens and AntalSvar cannot be null or empty.");
            }

            // Extract question-specific data (adjust regex pattern based on analysis of text)
            string[] questionKeywords = new string[] { "HELHETSOMDÖME", "UTVECKLING OCH LÄRANDE", "NORMER OCH VÄRDEN", "SAMVERKAN MED HEMMET", "KOST, RÖRELSE OCH HÄLSA" };
            HashSet<string> processedQuestions = new HashSet<string>();

            foreach (var keyword in questionKeywords)
            {
                var questionIndices = lines.Select((line, index) => new { line, index })
                                            .Where(x => x.line.Contains(keyword))
                                            .Select(x => x.index)
                                            .ToList();

                foreach (var questionIndex in questionIndices)
                {
                    var questionText = lines[questionIndex];
                    if (!processedQuestions.Contains(questionText) && questionIndex + 1 < lines.Count && int.TryParse(lines[questionIndex + 1], out int andelInstammer))
                    {
                        if (!questionText.Contains("Pojke") && !questionText.Contains("Flicka"))
                        {
                            var question = new Question
                            {
                                FrageText = questionText,
                                AndelInstammer = andelInstammer,
                                Year = DateTime.Now.Year // Adjust this to extract the actual year if available
                            };
                            malibu.Questions.Add(question);
                            processedQuestions.Add(questionText);
                        }
                    }
                }
            }

            if (malibu.Questions.Count == 0)
            {
                Console.WriteLine("Warning: No questions extracted.");
            }

            return malibu;
        }
    }
    }
