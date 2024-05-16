using EFCore.BulkExtensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using CsvHelper;
using CsvHelper.Configuration;
using MasterKinder.Data;
using MasterKinder.Models;
using System.Globalization;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace MasterKinder.Pages
{
    public class CsvService : PageModel
    {
        private readonly ILogger<CsvService> _logger;
        private readonly AppDbContext _context;

        public CsvService(ILogger<CsvService> logger, AppDbContext dbContext)
        {
            _logger = logger;
            _context = dbContext;
        }

        [BindProperty]
        public List<SurveyResponse> SurveyResponses { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            string filePath = @"C:\Users\Nils-\Downloads\UND_FSK (2).csv";

            var stopwatch = new System.Diagnostics.Stopwatch();
            stopwatch.Start();

            SurveyResponses = await ReadSurveyResponsesFromCsv(filePath);
            await AddSurveyResponsesToDbAsync(SurveyResponses, _context);

            stopwatch.Stop();
            var elapsedTime = stopwatch.Elapsed;
            _logger.LogInformation($"Time taken to insert data: {elapsedTime}");

            return Page();
        }

        public async Task<List<SurveyResponse>> ReadSurveyResponsesFromCsv(string filePath)
        {
            var surveyResponses = new List<SurveyResponse>();
            int rowCount = 0;
            int logCount = 0;
            int expectedColumnCount = 27;

            var config = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                HasHeaderRecord = true,
                MissingFieldFound = null,
                HeaderValidated = null,
                IgnoreBlankLines = true,
                Delimiter = ","
            };

            using (var reader = new StreamReader(filePath, Encoding.UTF8))
            using (var csv = new CsvReader(reader, config))
            {
                try
                {
                    // Read the header row to determine column indices
                    csv.Read();
                    csv.ReadHeader();

                    while (csv.Read())
                    {
                        var row = csv.Context.Parser.Record;

                        // Ensure the row has enough columns before processing
                        if (row.Length < expectedColumnCount)
                        {
                            _logger.LogWarning($"Row has fewer columns than expected: {row.Length} columns found. Row data: {string.Join(", ", row)}");
                            continue;
                        }

                        // Ensure proper conversion of each field
                        var surveyResponse = new SurveyResponse
                        {
                            AvserAr = row[0],
                           
                            Stadsdelsnamnd = row[3],
                            Forskoleenhet = row[4],
                            Fragaomradestext = row[8],
                            Fragetext = SafeGetField(row, 10, 11),
                            SvarsalternativText = SafeGetSvarsalternativText(row),
                           
                        };
                        surveyResponses.Add(surveyResponse);

                        // Log only the first 10 records to avoid too much logging
                        

                        rowCount++;
                    }

                    _logger.LogInformation($"Total rows processed: {rowCount}");
                }
                catch (HeaderValidationException ex)
                {
                    _logger.LogError($"Header validation error: {ex.Message}");
                }
                catch (ReaderException ex)
                {
                    _logger.LogError($"Reader error: {ex.Message}");
                }
                catch (Exception ex)
                {
                    _logger.LogError($"Error reading CSV file: {ex.Message}");
                }
            }

            return surveyResponses;
        }

        private string SafeGetField(string[] row, int primaryIndex, int fallbackIndex = -1)
        {
            try
            {
                string value = row[primaryIndex];
                if (fallbackIndex != -1 && Regex.IsMatch(value, @"^\d+$")) // Check if the value contains only digits
                {
                    return row[fallbackIndex];
                }
                return value;
            }
            catch
            {
                return "Unknown";
            }
        }

        private string SafeGetSvarsalternativText(string[] row)
        {
            int startIndex = 18;
            int endIndex = 22;

            for (int i = startIndex; i <= endIndex; i++)
            {
                if (i < row.Length && !Regex.IsMatch(row[i], @"^\d+$") && row[i] != "Påstående fråga")
                {
                    return row[i];
                }
                // Check if the value at index i is "Påstående fråga", then use the index five columns after
                if (i < row.Length && row[i] == "Påstående fråga" && (i + 5) < row.Length && !Regex.IsMatch(row[i + 5], @"^\d+$"))
                {
                    return row[i + 5];
                }
            }

            // If all the relevant columns are numeric or contain "Påstående fråga", check column 23
            if (row.Length > 23 && !Regex.IsMatch(row[23], @"^\d+$"))
            {
                return row[23];
            }

            return "Unknown";
        }

        public async Task AddSurveyResponsesToDbAsync(List<SurveyResponse> surveyResponses, AppDbContext context)
        {
            try
            {
                await context.BulkInsertAsync(surveyResponses, new BulkConfig { BatchSize = 5000 });
                _logger.LogInformation($"Successfully inserted {surveyResponses.Count} survey responses into the database.");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error inserting data into the database: {ex.Message}");
            }
        }
    }
}
