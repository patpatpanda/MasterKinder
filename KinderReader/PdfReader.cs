using System;
using System.IO;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using KinderReader.Models;
using MasterKinder.Data;
using Microsoft.Extensions.DependencyInjection;
using UglyToad.PdfPig;

namespace KinderReader
{
    public class PdfReader
    {
        public static async Task ReadPdfFromUrl(string pdfUrl, IServiceProvider services, int forskolanId)
        {
            using var httpClient = new HttpClient();
            var response = await httpClient.GetAsync(pdfUrl);

            if (response.IsSuccessStatusCode)
            {
                var pdfBytes = await response.Content.ReadAsByteArrayAsync();
                using var pdfStream = new MemoryStream(pdfBytes);

                using var document = PdfDocument.Open(pdfStream);
                var page = document.GetPage(1);
                var text = page.Text;

                var data = ExtractDataFromText(text);

                using var scope = services.CreateScope();
                var context = scope.ServiceProvider.GetRequiredService<MrDb>();

                var pdfResult = new PdfResult
                {
                    ForskolanId = forskolanId,
                    Helhetsomdome = data.Helhetsomdome,
                    AntalSvar = data.AntalSvar,
                    Svarsfrekvens = data.Svarsfrekvens
                };

                context.PdfResults.Add(pdfResult);
                await context.SaveChangesAsync();
            }
            else
            {
                Console.WriteLine($"Failed to download PDF from URL: {pdfUrl}");
            }
        }

        private static (string Helhetsomdome, string AntalSvar, string Svarsfrekvens) ExtractDataFromText(string text)
        {
            string helhetsomdome = Regex.Match(text, @"HELHETSOMDÖME\s+(\d+)").Groups[1].Value;
            string antalSvar = Regex.Match(text, @"(\d+)\s+svar").Groups[1].Value;
            string svarsfrekvens = Regex.Match(text, @"svarsfrekvens\s+(\d+)\s*%").Groups[1].Value;

            return (helhetsomdome, antalSvar, svarsfrekvens);
        }
    }
}
