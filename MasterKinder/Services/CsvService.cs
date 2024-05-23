﻿using Azure.Storage.Blobs;
using CsvHelper;
using CsvHelper.Configuration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;
using Azure.Storage.Blobs.Models;
using MasterKinder.Models;
using MasterKinder.Data;
using CsvHelper.TypeConversion;
using EFCore.BulkExtensions;

public class CsvService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<CsvService> _logger;
    private readonly AppDbContext _context;
    private readonly string _blobUri;

    public CsvService(IServiceProvider serviceProvider, ILogger<CsvService> logger, AppDbContext context, IConfiguration configuration)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
        _context = context;
        _blobUri = configuration["BlobUri"];
    }

    public async Task LoadCsvToDatabaseAsync()
    {
        _logger.LogInformation("Starting CSV download and import process.");
        await DownloadAndInsertCsvFromBlobAsync();
        _logger.LogInformation("CSV download and import process completed.");
    }

    private async Task DownloadAndInsertCsvFromBlobAsync()
    {
        _logger.LogInformation("Downloading CSV file from blob storage.");
        BlobClient blobClient = new BlobClient(new Uri(_blobUri));
        BlobDownloadInfo download = await blobClient.DownloadAsync();
        _logger.LogInformation("CSV file downloaded successfully.");

        using (StreamReader reader = new StreamReader(download.Content, Encoding.GetEncoding("iso-8859-1"), detectEncodingFromByteOrderMarks: false))
        using (CsvReader csv = new CsvReader(reader, new CsvConfiguration(CultureInfo.InvariantCulture)
        {
            HasHeaderRecord = true,
            BadDataFound = null // Ignore bad data and continue
        }))
        {
            csv.Context.RegisterClassMap<SurveyResponseMap>();

            const int batchSize = 10000; // Set batch size to 10000
            List<SurveyResponse> batch = new List<SurveyResponse>(batchSize);
            int totalRecords = 0;

            _logger.LogInformation("Starting to read and process CSV records.");

            while (await csv.ReadAsync())
            {
                try
                {
                    var record = csv.GetRecord<SurveyResponse>();
                    batch.Add(record);
                    totalRecords++;

                    if (batch.Count >= batchSize)
                    {
                        await InsertBatchAsync(batch, totalRecords);
                        batch.Clear();
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error reading CSV record. Skipping invalid record.");
                }
            }

            if (batch.Count > 0)
            {
                await InsertBatchAsync(batch, totalRecords);
            }

            _logger.LogInformation($"Total records inserted: {totalRecords}");
        }
    }

    private async Task InsertBatchAsync(List<SurveyResponse> batch, int totalRecords)
    {
        using (var scope = _serviceProvider.CreateScope())
        {
            try
            {
                var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                context.SurveyResponses.AddRange(batch);
                await context.SaveChangesAsync();
                _logger.LogInformation($"Inserted batch of {batch.Count} records, total records inserted: {totalRecords}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error inserting batch of records. Total records inserted: {totalRecords}");
            }
        }
    }

    public List<string> GetForskoleverksamheter()
    {
        return _context.SurveyResponses
            .Select(sr => sr.Forskoleverksamhet)
            .Distinct()
            .ToList();
    }

    public List<string> GetQuestions()
    {
        return _context.SurveyResponses
            .Select(sr => sr.Fragetext)
            .Distinct()
            .ToList();
    }

    public Dictionary<string, double> CalculateResponsePercentages(string questionText, string forskoleverksamhet)
    {
        var query = _context.SurveyResponses.AsQueryable();

        if (!string.IsNullOrEmpty(forskoleverksamhet))
        {
            query = query.Where(sr => sr.Forskoleverksamhet == forskoleverksamhet);
        }

        var totalResponses = query
            .Where(sr => sr.Fragetext == questionText)
            .Sum(sr => sr.Utfall);

        if (totalResponses == 0)
        {
            _logger.LogInformation($"No responses found for question '{questionText}' with forskoleverksamhet '{forskoleverksamhet}'");
            return new Dictionary<string, double>();
        }

        var responseCounts = query
            .Where(sr => sr.Fragetext == questionText)
            .GroupBy(sr => sr.SvarsalternativText)
            .Select(g => new
            {
                Response = g.Key,
                Count = g.Sum(sr => sr.Utfall)
            })
            .ToList();

        _logger.LogInformation($"Total responses for '{questionText}' with forskoleverksamhet '{forskoleverksamhet}': {totalResponses}");
        foreach (var response in responseCounts)
        {
            _logger.LogInformation($"Response: {response.Response}, Count: {response.Count}");
        }

        var responsePercentages = new Dictionary<string, double>();

        foreach (var response in responseCounts)
        {
            var key = GetResponseText(response.Response);
            if (responsePercentages.ContainsKey(key))
            {
                responsePercentages[key] += (double)response.Count / totalResponses * 100;
            }
            else
            {
                responsePercentages[key] = (double)response.Count / totalResponses * 100;
            }
        }

        foreach (var response in responsePercentages)
        {
            _logger.LogInformation($"Response: {response.Key}, Percentage: {response.Value}%");
        }

        return responsePercentages;
    }

    private string GetResponseText(string response)
    {
        var responseMapping = new Dictionary<string, string>
        {
            { "1", "Instämmer inte alls" },
            { "2", "Instämmer i liten utsträckning" },
            { "3", "Instämmer till viss del" },
            { "4", "Instämmer i stor utsträckning" },
            { "5", "Instämmer helt" },
            { "Instämmer inte alls", "Instämmer inte alls" },
            { "Vet ej", "Vet ej" },
            { "Övrig", "Övrig" }
        };

        return responseMapping.ContainsKey(response) ? responseMapping[response] : response;
    }

    public double CalculateSpecificResponsePercentage(string questionText, string responseText, string forskoleverksamhet)
    {
        var query = _context.SurveyResponses.AsQueryable();

        if (!string.IsNullOrEmpty(forskoleverksamhet))
        {
            query = query.Where(sr => sr.Forskoleverksamhet == forskoleverksamhet);
        }

        var totalResponses = query
            .Where(sr => sr.Fragetext == questionText)
            .Sum(sr => sr.Utfall);

        if (totalResponses == 0)
        {
            return 0;
        }

        var specificResponseCount = query
            .Where(sr => sr.Fragetext == questionText && sr.SvarsalternativText == responseText)
            .Sum(sr => sr.Utfall);

        return (double)specificResponseCount / totalResponses * 100;
    }

    public double CalculateOverallSatisfactionPercentage(string questionText, string forskoleverksamhet)
    {
        var query = _context.SurveyResponses.AsQueryable();

        if (!string.IsNullOrEmpty(forskoleverksamhet))
        {
            query = query.Where(sr => sr.Forskoleverksamhet == forskoleverksamhet);
        }

        var totalResponses = query
            .Where(sr => sr.Fragetext == questionText)
            .Sum(sr => sr.Utfall);

        if (totalResponses == 0)
        {
            return 0;
        }

        var satisfiedResponses = query
            .Where(sr => sr.Fragetext == questionText && sr.GraderingSvarsalternativ == "Nöjd")
            .Sum(sr => sr.Utfall);

        return (double)satisfiedResponses / totalResponses * 100;
    }

    public Dictionary<string, int> CountResponsesByGender(string forskoleverksamhet)
    {
        var query = _context.SurveyResponses.AsQueryable();

        if (!string.IsNullOrEmpty(forskoleverksamhet))
        {
            query = query.Where(sr => sr.Forskoleverksamhet == forskoleverksamhet);
        }

        return query
            .GroupBy(sr => sr.Kon)
            .Select(g => new { Gender = g.Key, Count = g.Sum(sr => sr.Utfall) })
            .ToDictionary(g => g.Gender, g => g.Count);
    }
}


public class SurveyResponseMap : ClassMap<SurveyResponse>
{
    public SurveyResponseMap()
    {
        Map(m => m.AvserAr).Name("AvserAr");
        Map(m => m.ResultatkategoriKod).Name("ResultatkategoriKod");
        Map(m => m.ResultatkategoriNamn).Name("ResultatkategoriNamn");
        Map(m => m.Stadsdelsnamnd).Name("Stadsdelsnämnd");
        Map(m => m.Forskoleenhet).Name("Förskoleenhet");
        Map(m => m.Organisatoriskenhetskod).Name("Organisatoriskenhetskod");
        Map(m => m.Forskoleverksamhet).Name("Förskoleverksamhet");
        Map(m => m.RegiformNamn).Name("RegiformNamn");
        Map(m => m.FragaomradeNr).Name("FrågeområdeNr").TypeConverter<SafeIntConverter>();
        Map(m => m.Fragaomradestext).Name("Frågeområdestext");
        Map(m => m.FragaNr).Name("FrågeNr").TypeConverter<SafeIntConverter>();
        Map(m => m.Fragetext).Name("Frågetext");
        Map(m => m.Kortfragetext).Name("Kortfrågetext");
        Map(m => m.SvarsalternativTyp).Name("SvarsalternativTyp");
        Map(m => m.Fragetyp).Name("Frågetyp");
        Map(m => m.Fragkategori).Name("Frågekategori");
        Map(m => m.AntalSvarsalternativ).Name("AntalSvarsalternativ").TypeConverter<SafeIntConverter>();
        Map(m => m.SvarsalternativNr).Name("SvarsalternativNr").TypeConverter<SafeIntConverter>();
        Map(m => m.SvarsalternativText).Name("SvarsalternativText");
        Map(m => m.GraderingSvarsalternativ).Name("GraderingSvarsalternativ");
        Map(m => m.Enkatroll).Name("Enkätroll");
        Map(m => m.Respondentroll).Name("Respondentroll");
        Map(m => m.Kon).Name("Kön");
        Map(m => m.Utfall).Name("Utfall").TypeConverter<SafeIntConverter>();
        Map(m => m.TotalVarde).Name("TotalVarde").TypeConverter<SafeIntConverter>();
        Map(m => m.TotalVarde_ExklVetEj).Name("TotalVarde_ExklVetEj").TypeConverter<SafeIntConverter>();
    }
}

public class SafeIntConverter : Int32Converter
{
    public override object ConvertFromString(string text, IReaderRow row, MemberMapData memberMapData)
    {
        if (int.TryParse(text, out int result))
        {
            return result;
        }
        return 0; // Default value if parsing fails
    }
}
