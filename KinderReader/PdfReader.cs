//using MasterKinder.Data;
//using MasterKinder.Models;
//using Microsoft.Extensions.Logging;
//using System.Net.Http;
//using System.Text.RegularExpressions;
//using System.Threading.Tasks;
//using UglyToad.PdfPig;

//namespace KinderReader
//{
//    public class PdfReader
//    {
//        private readonly ILogger<PdfReader> _logger;
//        private readonly MrDb _context;

//        public PdfReader(ILogger<PdfReader> logger, MrDb context)
//        {
//            _logger = logger;
//            _context = context;
//        }

//        public async Task ReadPdfFromUrl(string pdfUrl, int forskolanId)
//        {
//            _logger.LogInformation($"Starting to read PDF from URL: {pdfUrl}");

//            try
//            {
//                using var httpClient = new HttpClient();
//                var pdfBytes = await httpClient.GetByteArrayAsync(pdfUrl);

//                using var document = PdfDocument.Open(pdfBytes);
//                var firstPageText = document.GetPage(1).Text;

//                var pdfResults = ExtractDataFromText(firstPageText, _logger);
//                SavePdfResults(forskolanId, pdfResults);

//                _logger.LogInformation($"Successfully processed PDF from URL: {pdfUrl}");
//            }
//            catch (Exception ex)
//            {
//                _logger.LogError($"Error processing PDF from URL: {pdfUrl}. Error: {ex.Message}");
//            }
//        }

//        private (string Helhetsomdome, string AntalSvar, string Svarsfrekvens) ExtractDataFromText(string text, ILogger logger)
//        {
//            logger.LogInformation($"Extracted text from PDF: {text}");

//            string helhetsomdome = Regex.Match(text, @"HELHETSOMDÖME\s+(\d+)", RegexOptions.IgnoreCase).Groups[1].Value;
//            string antalSvar = Regex.Match(text, @"(\d+)\s+svar", RegexOptions.IgnoreCase).Groups[1].Value;
//            string svarsfrekvens = Regex.Match(text, @"svarsfrekvens\s+(\d+)\s*%", RegexOptions.IgnoreCase).Groups[1].Value;

//            logger.LogInformation($"Parsed Helhetsomdome: {helhetsomdome}, AntalSvar: {antalSvar}, Svarsfrekvens: {svarsfrekvens}");

//            return (helhetsomdome, antalSvar, svarsfrekvens);
//        }

//        private void SavePdfResults(int forskolanId, (string Helhetsomdome, string AntalSvar, string Svarsfrekvens) pdfResults)
//        {
//            var result = new PdfResult
//            {
//                ForskolanId = forskolanId,
//                Helhetsomdome = pdfResults.Helhetsomdome,
//                AntalSvar = pdfResults.AntalSvar,
//                Svarsfrekvens = pdfResults.Svarsfrekvens
//            };

//            _context.PdfResults.Add(result);
//            _context.SaveChanges();
//        }
//    }
//}
