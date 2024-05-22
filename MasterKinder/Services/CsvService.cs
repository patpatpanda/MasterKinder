using Azure.Storage.Blobs;
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
    private readonly AppDbContext _context;
    private readonly string _blobUri;
    private readonly ILogger<CsvService> _logger;

    public CsvService(AppDbContext context, IConfiguration configuration, ILogger<CsvService> logger)
    {
        _context = context;
        _blobUri = configuration["BlobUri"];
        _logger = logger;
    }

    public async Task LoadCsvToDatabaseAsync()
    {
        _logger.LogInformation("Starting CSV download and import process.");
        await DownloadAndInsertCsvFromBlobAsync(_blobUri);
        _logger.LogInformation("CSV download and import process completed.");
    }

    private async Task DownloadAndInsertCsvFromBlobAsync(string blobUri)
    {
        _logger.LogInformation("Downloading CSV file from blob storage.");
        BlobClient blobClient = new BlobClient(new Uri(blobUri));
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

            var records = new List<SurveyResponse>();

            _logger.LogInformation("Starting to read and process CSV records.");

            while (await csv.ReadAsync())
            {
                try
                {
                    var record = csv.GetRecord<dynamic>();
                    var surveyResponse = ConvertToSurveyResponse(record);
                    if (surveyResponse != null)
                    {
                        records.Add(surveyResponse);
                    }
                }
                catch (CsvHelper.ReaderException ex)
                {
                    _logger.LogError(ex, "Error reading CSV record. Skipping invalid record.");
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Unexpected error reading CSV record.");
                }
            }

            _logger.LogInformation($"Read {records.Count} records from CSV file.");

            const int batchSize = 50000; // Adjust batch size as needed

            for (int i = 0; i < records.Count; i += batchSize)
            {
                var batch = records.Skip(i).Take(batchSize).ToList();
                _context.BulkInsert(batch);
                _logger.LogInformation($"Inserted batch of {batch.Count} records, total records inserted: {i + batch.Count}");
            }

            _logger.LogInformation("All records have been inserted into the database.");
        }
    }

    private SurveyResponse ConvertToSurveyResponse(dynamic record)
    {
        try
        {
            return new SurveyResponse
            {
                AvserAr = Convert.ToString(record.AvserAr),
                ResultatkategoriKod = Convert.ToString(record.ResultatkategoriKod),
                ResultatkategoriNamn = Convert.ToString(record.ResultatkategoriNamn),
                Stadsdelsnamnd = Convert.ToString(record.Stadsdelsnämnd),
                Forskoleenhet = Convert.ToString(record.Förskoleenhet),
                Organisatoriskenhetskod = Convert.ToString(record.Organisatoriskenhetskod),
                Forskoleverksamhet = Convert.ToString(record.Förskoleverksamhet),
                RegiformNamn = Convert.ToString(record.RegiformNamn),
                FragaomradeNr = Convert.ToInt32(record.FrågeområdeNr),
                Fragaomradestext = Convert.ToString(record.Frågeområdestext),
                FragaNr = Convert.ToInt32(record.FrågeNr),
                Fragetext = Convert.ToString(record.Frågetext),
                Kortfragetext = Convert.ToString(record.Kortfrågetext),
                SvarsalternativTyp = Convert.ToString(record.SvarsalternativTyp),
                Fragetyp = Convert.ToString(record.Frågetyp),
                Fragkategori = Convert.ToString(record.Frågekategori),
                AntalSvarsalternativ = Convert.ToInt32(record.AntalSvarsalternativ),
                SvarsalternativNr = Convert.ToInt32(record.SvarsalternativNr),
                SvarsalternativText = Convert.ToString(record.SvarsalternativText),
                GraderingSvarsalternativ = Convert.ToString(record.GraderingSvarsalternativ),
                Enkatroll = Convert.ToString(record.Enkätroll),
                Respondentroll = Convert.ToString(record.Respondentroll),
                Kon = Convert.ToString(record.Kön),
                Utfall = Convert.ToInt32(record.Utfall),
                TotalVarde = Convert.ToInt32(record.TotalVarde),
                TotalVarde_ExklVetEj = Convert.ToInt32(record.TotalVarde_ExklVetEj)
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error converting CSV record to SurveyResponse.");
            return null; // Skip invalid records
        }
    }

    public Dictionary<string, int> CountResponsesPerQuestion()
    {
        return _context.SurveyResponses
            .GroupBy(sr => sr.Fragetext)
            .Select(g => new { Question = g.Key, Count = g.Count() })
            .ToDictionary(g => g.Question, g => g.Count);
    }
    public List<string> GetQuestions()
    {
        return _context.SurveyResponses
            .Select(sr => sr.Fragetext)
            .Distinct()
            .ToList();
    }
    public Dictionary<string, double> CalculateResponsePercentages(string questionText)
    {
        var totalResponses = _context.SurveyResponses
            .Where(sr => sr.Fragetext == questionText)
            .Sum(sr => sr.Utfall);

        if (totalResponses == 0)
        {
            _logger.LogInformation($"No responses found for question '{questionText}'");
            return new Dictionary<string, double>();
        }

        var responseCounts = _context.SurveyResponses
            .Where(sr => sr.Fragetext == questionText)
            .GroupBy(sr => sr.SvarsalternativText)
            .Select(g => new
            {
                Response = g.Key,
                Count = g.Sum(sr => sr.Utfall)
            })
            .ToList();

        _logger.LogInformation($"Total responses for '{questionText}': {totalResponses}");
        foreach (var response in responseCounts)
        {
            _logger.LogInformation($"Response: {response.Response}, Count: {response.Count}");
        }

        var responsePercentages = responseCounts.ToDictionary(
            x => x.Response,
            x => (double)x.Count / totalResponses * 100
        );

        foreach (var response in responsePercentages)
        {
            _logger.LogInformation($"Response: {response.Key}, Percentage: {response.Value}%");
        }

        return responsePercentages;
    }
    public List<string> GetAllQuestions()
    {
        return _context.SurveyResponses
            .Select(sr => sr.Fragetext)
            .Distinct()
            .ToList();
    }
    private string GetResponseText(string response)
    {
        var responseMapping = new Dictionary<string, string>
        {
            { "1", "Missnöjd" },
            { "2", "Neutral" },
            { "3", "Nöjd" },
            { "4", "Instämmer helt" },
            { "Instämmer inte alls", "Instämmer inte alls" },
            { "Vet ej", "Vet ej" },
            { "Övrig", "Övrig" }
        };

        return responseMapping.ContainsKey(response) ? responseMapping[response] : response;
    }

    public double CalculateSpecificResponsePercentage(string questionText, string responseText)
    {
        var totalResponses = _context.SurveyResponses
            .Where(sr => sr.Fragetext == questionText)
            .Count();

        if (totalResponses == 0)
        {
            return 0;
        }

        var specificResponseCount = _context.SurveyResponses
            .Where(sr => sr.Fragetext == questionText && sr.SvarsalternativText == responseText)
            .Count();

        return (double)specificResponseCount / totalResponses * 100;
    }

    public double CalculateOverallSatisfactionPercentage(string questionText)
    {
        var totalResponses = _context.SurveyResponses
            .Where(sr => sr.Fragetext == questionText)
            .Count();

        if (totalResponses == 0)
        {
            return 0;
        }

        var satisfiedResponses = _context.SurveyResponses
            .Where(sr => sr.Fragetext == questionText && sr.GraderingSvarsalternativ == "Nöjd")
            .Count();

        return (double)satisfiedResponses / totalResponses * 100;
    }

    public Dictionary<string, int> CountResponsesByGender()
    {
        return _context.SurveyResponses
            .GroupBy(sr => sr.Kon)
            .Select(g => new { Gender = g.Key, Count = g.Count() })
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

