using EFCore.BulkExtensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using CsvHelper;
using CsvHelper.Configuration;
using MasterKinder.Data;
using MasterKinder.Models;
using System.Globalization;

namespace MasterKinder.Pages
{
    public class CsvService : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly AppDbContext _context;

        public CsvService(ILogger<IndexModel> logger, AppDbContext dbContext)
        {
            _logger = logger;
            _context = dbContext;
        }

        public async Task<IActionResult> OnGetAsync()
        {
            string filePath = @"C:\Users\Nils-\Downloads\UND_FSK.csv";

            var stopwatch = new System.Diagnostics.Stopwatch();
            stopwatch.Start();

            var surveyResponses = await ReadSurveyResponsesFromCsv(filePath);
            await AddSurveyResponsesToDbAsync(surveyResponses, _context);

            stopwatch.Stop();
            var elapsedTime = stopwatch.Elapsed;
            _logger.LogInformation($"Time taken to insert data: {elapsedTime}");

            return Page();
        }

        public async Task<List<SurveyResponse>> ReadSurveyResponsesFromCsv(string filePath)
        {
            var surveyResponses = new List<SurveyResponse>();

            var config = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                HasHeaderRecord = true,
            };

            using (var reader = new StreamReader(filePath))
            using (var csv = new CsvReader(reader, config))
            {
                csv.Context.RegisterClassMap<SurveyResponseMap>();
                surveyResponses = csv.GetRecords<SurveyResponse>().ToList();
            }

            return surveyResponses;
        }

        public async Task AddSurveyResponsesToDbAsync(List<SurveyResponse> surveyResponses, AppDbContext context)
        {
            await context.BulkInsertAsync(surveyResponses);
        }
    }

    public class SurveyResponseMap : ClassMap<SurveyResponse>
    {
        public SurveyResponseMap()
        {
            Map(m => m.AvserAr).Index(0);
            Map(m => m.ResultatkategoriKod).Index(1);
            Map(m => m.ResultatkategoriNamn).Index(2);
            Map(m => m.Stadsdelsnamnd).Index(3);
            Map(m => m.Forskoleenhet).Index(4);
            Map(m => m.Organisatoriskenhetskod).Index(5);
            Map(m => m.Forskoleverksamhet).Index(6);
            Map(m => m.RegiformNamn).Index(7);
            Map(m => m.FragaomradeNr).Index(8);
            Map(m => m.Fragaomradestext).Index(9);
            Map(m => m.FragaNr).Index(10);
            Map(m => m.Fragetext).Index(11);
            Map(m => m.Kortfragetext).Index(12);
            Map(m => m.SvarsalternativTyp).Index(13);
            Map(m => m.Fragetype).Index(14);
            Map(m => m.Fragcategory).Index(15);
            Map(m => m.AntalSvarsalternativ).Index(16);
            Map(m => m.SvarsalternativNr).Index(17);
            Map(m => m.SvarsalternativText).Index(18);
            Map(m => m.GraderingSvarsalternativ).Index(19);
            Map(m => m.Enkatroll).Index(20);
            Map(m => m.Respondentroll).Index(21);
            Map(m => m.Kon).Index(22);
            Map(m => m.Utfall).Index(23);
            Map(m => m.TotalVarde).Index(24);
            Map(m => m.TotalVarde_ExklVetEj).Index(25);
        }
    }
}
