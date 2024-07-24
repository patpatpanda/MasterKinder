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
using System.Text.RegularExpressions;
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
                string pageText = PdfTextExtractor.GetTextFromPage(reader, i);
                if (i >= 27 && !ContainsSpindeldiagram(pageText)) // Start extracting from page 27
                {
                    pdfText += pageText;
                }
                else
                {
                    Console.WriteLine($"Skipped page {i} due to spindeldiagram or it's before page 27.");
                }
            }
        }

        return pdfText;
    }

    private bool ContainsSpindeldiagram(string pageText)
    {
        // Logic to identify pages with spindeldiagram, this is a placeholder.
        // You can update this based on specific keywords or patterns that identify a spindeldiagram page.
        return pageText.Contains("Spindeldiagram", StringComparison.OrdinalIgnoreCase);
    }

    public void SaveSurveyResultToDatabase(SurveyResult surveyResult)
    {
        try
        {
            var existingEntry = _dbContext.SurveyResults.FirstOrDefault(p =>
                p.AvserAr == surveyResult.AvserAr &&
                p.Forskolenhet == surveyResult.Forskolenhet &&
                p.FragaNr == surveyResult.FragaNr &&
                p.SvarsalternativNr == surveyResult.SvarsalternativNr);
            if (existingEntry == null)
            {
                _dbContext.SurveyResults.Add(surveyResult);
                _dbContext.SaveChanges();
                Console.WriteLine($"Saved survey result for {surveyResult.Forskolenhet} to the database.");
            }
            else
            {
                Console.WriteLine($"Duplicate entry found for {surveyResult.Forskolenhet} with question number {surveyResult.FragaNr} and answer number {surveyResult.SvarsalternativNr}, skipping save.");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred while saving survey result to the database: {ex.Message}");
        }
    }

    public async Task ScrapeAndSaveSurveyData(string url)
    {
        var pdfLinks = await ScrapePdfLinksAsync(url);

        foreach (var pdfLink in pdfLinks)
        {
            try
            {
                var pdfText = await DownloadAndExtractPdfText(pdfLink);
                if (!string.IsNullOrEmpty(pdfText))
                {
                    var surveyResults = ExtractDataFromText(pdfText);
                    foreach (var result in surveyResults)
                    {
                        SaveSurveyResultToDatabase(result);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while processing {pdfLink}: {ex.Message}");
            }
        }
    }

    public List<SurveyResult> ExtractDataFromText(string pdfText)
    {
        var surveyResults = new List<SurveyResult>();
        var lines = pdfText.Split('\n').Select(line => line.Trim()).ToList();

        // Log first few lines for initial debugging
        Console.WriteLine("PDF Text (First 20 lines for debugging):");
        for (int i = 0; i < Math.Min(20, lines.Count); i++)
        {
            Console.WriteLine(lines[i]);
        }

        var pdfData = new SurveyResult
        {
            FragaOmradeText = "Standard text",  // Assign a default value to FragaOmradeText
            Forskoleverksamhet = "Standard verksamhet", // Assign a default value to Forskoleverksamhet
            ResultatkategoriKod = "Default",
            ResultatkategoriNamn = "Default",
            Stadsdelsnamnd = "Default",
            RegiformNamn = "Default",
            EnkatRoll = "Default",
            RespondentRoll = "Default",
            Kon = "Default"
        };

        // Extracting the name and general info
        var nameIndex = lines.FindIndex(line => line.Contains("Vårdnadshavare Förskola"));
        if (nameIndex != -1 && nameIndex + 1 < lines.Count)
        {
            pdfData.Forskolenhet = lines[nameIndex + 1].Trim();
        }
        else
        {
            Console.WriteLine("Failed to extract name. PDF text:");
            for (int i = 0; i < lines.Count; i++)
            {
                Console.WriteLine(lines[i]);
            }
            throw new InvalidOperationException("Namn cannot be null or empty.");
        }

        // Extract overall rating (Helhetsomdome) and response rate
        var helhetsomdomeLineIndex = lines.FindIndex(line => line.Contains("Genomförd av Origo Group"));
        if (helhetsomdomeLineIndex != -1 && helhetsomdomeLineIndex + 1 < lines.Count)
        {
            var responseLine = lines[helhetsomdomeLineIndex + 1];
            var responseParts = responseLine.Split(new[] { ',', '%' }, StringSplitOptions.RemoveEmptyEntries);
            if (responseParts.Length >= 2)
            {
                pdfData.Utfall = ParseNullableDouble(responseParts[1]);
                pdfData.TotalVarde = ParseNullableDouble(responseParts[0]);
                Console.WriteLine($"Extracted helhetsomdome: {pdfData.Utfall}, response rate: {pdfData.TotalVarde}");
            }
            else
            {
                Console.WriteLine("Failed to extract helhetsomdome and response rate.");
            }
        }

        // Extract other data points starting from page 27
        ExtractAndAssignPercentage(pdfData, lines, "Jag upplever att mitt barn utvecklas och lär i förskolan", nameof(pdfData.FragaNr1));
        ExtractAndAssignPercentage(pdfData, lines, "Jag upplever att mitt barns språkliga förmåga utvecklas i förskolan", nameof(pdfData.FragaNr2));
        ExtractAndAssignPercentage(pdfData, lines, "Jag upplever att mitt barn får det stöd som behövs i förskolan", nameof(pdfData.FragaNr3));
        ExtractAndAssignPercentage(pdfData, lines, "Jag upplever att förskolan bidrar till att mitt barn visar ett intresse för hållbar utveckling", nameof(pdfData.FragaNr4));
        ExtractAndAssignPercentage(pdfData, lines, "Jag får information om mitt barns utveckling och lärande", nameof(pdfData.FragaNr5));
        ExtractAndAssignPercentage(pdfData, lines, "Jag upplever att mitt barn känner sig tryggt på förskolan", nameof(pdfData.FragaNr6));
        ExtractAndAssignPercentage(pdfData, lines, "Jag upplever att förskolan bidrar till att mitt barn utvecklar förmåga till empati, tolerans och omtanke", nameof(pdfData.FragaNr7));
        ExtractAndAssignPercentage(pdfData, lines, "Jag upplever att personalen visar omsorg om mitt barn", nameof(pdfData.FragaNr8));
        ExtractAndAssignPercentage(pdfData, lines, "Jag upplever att förskolan bidrar till att mitt barn utvecklar en positiv bild av sig själv", nameof(pdfData.FragaNr9));
        ExtractAndAssignPercentage(pdfData, lines, "Jag upplever att förskolan medvetet främjar alla barns möjligheter att utvecklas på lika villkor oavsett kön", nameof(pdfData.FragaNr10));
        ExtractAndAssignPercentage(pdfData, lines, "Jag upplever att förskolans ledning är tillgängliga vid behov", nameof(pdfData.FragaNr11));
        ExtractAndAssignPercentage(pdfData, lines, "Jag upplever att personalen bemöter mig på ett sätt som skapar förtroende och tillit", nameof(pdfData.FragaNr12));
        ExtractAndAssignPercentage(pdfData, lines, "Jag är nöjd med informationen jag får om maten som serveras på förskolan", nameof(pdfData.FragaNr13));
        ExtractAndAssignPercentage(pdfData, lines, "Jag upplever att förskolan bidrar till att mitt barn dagligen deltar i fysiska aktiviteter", nameof(pdfData.FragaNr14));

        surveyResults.Add(pdfData);

        return surveyResults;
    }

    private void ExtractAndAssignPercentage(SurveyResult surveyResult, List<string> lines, string question, string propertyName)
    {
        var index = lines.FindIndex(line => line.Contains(question, StringComparison.OrdinalIgnoreCase));
        if (index != -1)
        {
            // Search for a percentage within the next few lines
            for (int i = index + 1; i < Math.Min(index + 6, lines.Count); i++)
            {
                if (lines[i].Contains("instämmer (%)", StringComparison.OrdinalIgnoreCase))
                {
                    var percentageMatch = Regex.Match(lines[i + 1], @"(\d+)");
                    if (percentageMatch.Success)
                    {
                        var percentage = double.Parse(percentageMatch.Value.Trim());
                        var property = surveyResult.GetType().GetProperty(propertyName);
                        if (property != null && property.PropertyType == typeof(double?))
                        {
                            property.SetValue(surveyResult, percentage);
                            Console.WriteLine($"Extracted percentage for field: {question} - {percentage}%");
                        }
                        return;
                    }
                }
            }
        }

        Console.WriteLine($"Failed to extract percentage for field: {question}");
    }

    private int? ParseNullableInt(string input)
    {
        if (int.TryParse(input, out int result))
        {
            return result;
        }
        return null;
    }

    private double? ParseNullableDouble(string input)
    {
        if (double.TryParse(input, out double result))
        {
            return result;
        }
        return null;
    }

    private string ParseString(string input)
    {
        return string.IsNullOrWhiteSpace(input) ? null : input;
    }


}
