using HtmlAgilityPack;
using iTextSharp.text.pdf;
using iTextSharp.text.pdf.parser;
using MasterKinder.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using MasterKinder.Models;

public class PdfScrapingService
{
    private readonly HttpClient _httpClient;
    private readonly MrDb _dbContext;

    public PdfScrapingService(HttpClient httpClient, MrDb dbContext)
    {
        _httpClient = httpClient;
        _dbContext = dbContext;
    }

    public async Task<List<string>> ScrapePdfLinksAsync(string url)
    {
        var html = await _httpClient.GetStringAsync(url);
        var htmlDocument = new HtmlDocument();
        htmlDocument.LoadHtml(html);

        var pdfLinks = new List<string>();
        var nodes = htmlDocument.DocumentNode.SelectNodes("//a[contains(@href, '.pdf')]");

        if (nodes != null)
        {
            pdfLinks.AddRange(nodes.Select(node => node.GetAttributeValue("href", string.Empty)));
        }

        return pdfLinks;
    }

    public async Task<string> DownloadAndExtractPdfText(string pdfUrl)
    {
        var pdfBytes = await _httpClient.GetByteArrayAsync(pdfUrl);
        var pdfText = string.Empty;

        using (var stream = new MemoryStream(pdfBytes))
        using (var reader = new PdfReader(stream))
        {
            for (int i = 1; i <= reader.NumberOfPages; i++)
            {
                pdfText += PdfTextExtractor.GetTextFromPage(reader, i);
            }
        }

        return pdfText;
    }

    public void SavePdfDataToDatabase(PdfDataAll pdfData)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(pdfData.Namn))
            {
                throw new InvalidOperationException("Namn cannot be null or empty.");
            }

            if (string.IsNullOrWhiteSpace(pdfData.Helhetsomdome))
            {
                throw new InvalidOperationException("Helhetsomdome cannot be null or empty.");
            }

            var existingEntry = _dbContext.PdfDataAlls.FirstOrDefault(p => p.Namn == pdfData.Namn && p.AntalSvar == pdfData.AntalSvar);
            if (existingEntry == null)
            {
                _dbContext.PdfDataAlls.Add(pdfData);
                _dbContext.SaveChanges();
                Console.WriteLine($"Saved PDF data for {pdfData.Namn} to the database.");
            }
            else
            {
                Console.WriteLine($"Duplicate entry found for {pdfData.Namn} with {pdfData.AntalSvar} responses, skipping save.");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred while saving PDF data to the database: {ex.Message}");
        }
    }

    public async Task ScrapeAndSavePdfData(string url)
    {
        var pdfLinks = await ScrapePdfLinksAsync(url);

        foreach (var pdfLink in pdfLinks)
        {
            try
            {
                var pdfText = await DownloadAndExtractPdfText(pdfLink);
                if (!string.IsNullOrEmpty(pdfText))
                {
                    var pdfDataList = ExtractDataFromText(pdfText);
                    foreach (var pdfData in pdfDataList)
                    {
                        SavePdfDataToDatabase(pdfData);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while processing {pdfLink}: {ex.Message}");
            }
        }
    }

    public List<PdfDataAll> ExtractDataFromText(string pdfText)
    {
        var pdfDataList = new List<PdfDataAll>();
        var lines = pdfText.Split('\n').Select(line => line.Trim()).ToList();

        // Log first few lines for initial debugging
        Console.WriteLine("PDF Text (First 20 lines for debugging):");
        for (int i = 0; i < Math.Min(20, lines.Count); i++)
        {
            Console.WriteLine(lines[i]);
        }

        // Extracting the name
        var nameIndex = lines.FindIndex(line => line.Contains("Vårdnadshavare Förskola") || line.Contains("Vårdnadshavare Familjedaghem"));
        string namn = null;
        if (nameIndex != -1 && nameIndex + 1 < lines.Count)
        {
            namn = lines[nameIndex + 1].Trim();
            Console.WriteLine($"Extracted name: {namn}");
        }

        if (string.IsNullOrWhiteSpace(namn))
        {
            Console.WriteLine("Failed to extract name. PDF text:");
            for (int i = 0; i < lines.Count; i++)
            {
                Console.WriteLine(lines[i]);
            }
            throw new InvalidOperationException("Namn cannot be null or empty.");
        }

        // Extract overall rating (Helhetsomdome)
        var helhetsomdomeLineIndex = lines.FindIndex(line => line.Contains("HELHETSOMDÖME"));
        string helhetsomdome = null;
        if (helhetsomdomeLineIndex != -1 && helhetsomdomeLineIndex + 1 < lines.Count)
        {
            helhetsomdome = lines[helhetsomdomeLineIndex + 1].Trim();
            Console.WriteLine($"Extracted helhetsomdome: {helhetsomdome}");
        }
        else
        {
            // Log additional lines for debugging if the expected line is not found
            Console.WriteLine("Failed to extract helhetsomdome. PDF text:");
            for (int i = 0; i < lines.Count; i++)
            {
                Console.WriteLine(lines[i]);
            }
            throw new InvalidOperationException("Helhetsomdome cannot be null or empty.");
        }

        // Extract response rate and number of responses
        var svarsfrekvensLineIndex = lines.FindIndex(line => line.Contains("svar") && line.Contains("%"));
        int svarsfrekvens = 0;
        int antalSvar = 0;
        if (svarsfrekvensLineIndex != -1)
        {
            var svarsfrekvensLine = lines[svarsfrekvensLineIndex];
            var delar = svarsfrekvensLine.Split(',');
            if (delar.Length > 1)
            {
                svarsfrekvens = int.Parse(delar[1].Replace("%", "").Trim());
                antalSvar = int.Parse(delar[0].Split(' ')[0].Trim());
                Console.WriteLine($"Extracted svarsfrekvens: {svarsfrekvens}, antal svar: {antalSvar}");
            }
        }
        else
        {
            // Log additional lines for debugging if the expected line is not found
            Console.WriteLine("Failed to extract svarsfrekvens. PDF text:");
            for (int i = 0; i < lines.Count; i++)
            {
                Console.WriteLine(lines[i]);
            }
            throw new InvalidOperationException("Svarsfrekvens and AntalSvar cannot be null or empty.");
        }

        // Extracting questions data
        for (int i = 0; i < lines.Count; i++)
        {
            if (lines[i].Contains("Jag upplever att mitt barn utvecklas och lär i förskolan") ||
                lines[i].Contains("Jag upplever att mitt barns språkliga förmåga utvecklas i förskolan") ||
                lines[i].Contains("Jag upplever att mitt barn får det stöd som behövs i förskolan"))
            {
                var pdfData = new PdfDataAll
                {
                    Namn = namn,
                    Helhetsomdome = helhetsomdome,
                    Svarsfrekvens = svarsfrekvens,
                    AntalSvar = antalSvar,
                    FrageText = lines[i].Trim()
                };

                Console.WriteLine($"Extracted FrageText: {pdfData.FrageText}");

                // Extracting "Instämmer helt" and "Instämmer" while avoiding year values
                var values = ExtractValues(lines, i);
                pdfData.InstammerHelt = values.Item1;
                pdfData.Instammer = values.Item2;

                Console.WriteLine($"Extracted InstammerHelt: {pdfData.InstammerHelt}");
                Console.WriteLine($"Extracted Instammer: {pdfData.Instammer}");

                pdfDataList.Add(pdfData);
            }
        }

        return pdfDataList;
    }

    private Tuple<int, int> ExtractValues(List<string> lines, int startIndex)
    {
        var yearsToIgnore = new HashSet<int> { 2022, 2023, 2024 };
        for (int i = startIndex; i < lines.Count; i++)
        {
            var parts = lines[i].Split(' ');
            for (int j = 0; j < parts.Length; j++)
            {
                if (int.TryParse(parts[j], out int number) && !yearsToIgnore.Contains(number))
                {
                    // Check if the next part contains the percentage symbol
                    if (parts.Length > j + 1 && parts[j + 1].Contains('%'))
                    {
                        return Tuple.Create(number, int.Parse(parts[j + 2]));
                    }
                }
            }
        }
        return Tuple.Create(0, 0);
    }
}
