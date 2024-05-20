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
using Microsoft.EntityFrameworkCore;

namespace MasterKinder.Pages
{
    public class IndexModel : PageModel
    {
        private readonly AppDbContext _context;
        private readonly ILogger<IndexModel> _logger;

        public IndexModel(AppDbContext context, ILogger<IndexModel> logger)
        {
            _context = context;
            _logger = logger;
        }
        public IList<SurveyResponse> ForskolaFragaList { get; set; }
        public int PageIndex { get; set; }
        public int TotalPages { get; set; }
        public double VetEjPercentage { get; set; }

        [BindProperty(SupportsGet = true)]
        public string AvserAr { get; set; }

        [BindProperty(SupportsGet = true)]
        public string Stadsdelsnamnd { get; set; }

        [BindProperty(SupportsGet = true)]
        public string Forskoleenhet { get; set; }

        [BindProperty(SupportsGet = true)]
        public string SpecificQuestion { get; set; }

        public List<string> UniqueAvserAr { get; set; }
        public List<string> UniqueStadsdelsnamnd { get; set; }
        public List<string> UniqueForskoleenhet { get; set; }
        public List<string> UniqueSpecificQuestions { get; set; }
        public List<ResponsePercentage> ResponsePercentages { get; set; }

        private static readonly List<string> ExclusionWords = new List<string>
        {
            "K�n",
            "K�ns�verskridande identitet eller uttryck",
            "Religion eller trosuppfattning",
            "Funktionsneds�ttning",
            "Sexuell l�ggning",
            "�lder",
            "P�st�ende fr�ga",
            "Enk�tfr�ga"
        };

        public async Task OnGetAsync(int pageIndex = 1)
        {
            UniqueAvserAr = await _context.SurveyResponses.Select(f => f.AvserAr.ToString()).Distinct().ToListAsync();
            UniqueStadsdelsnamnd = await _context.SurveyResponses.Select(f => f.Stadsdelsnamnd).Distinct().ToListAsync();
            UniqueForskoleenhet = await _context.SurveyResponses.Select(f => f.Forskoleenhet).Distinct().ToListAsync();
            UniqueSpecificQuestions = await _context.SurveyResponses.Select(f => f.Fragetext).Distinct().ToListAsync();

            int pageSize = 10; // Antal poster per sida
            var query = _context.SurveyResponses.AsQueryable();

            if (!string.IsNullOrEmpty(AvserAr))
            {
                _logger.LogInformation("Filtering by AvserAr: {AvserAr}", AvserAr);
                query = query.Where(f => f.AvserAr.ToString() == AvserAr);
            }

            if (!string.IsNullOrEmpty(Stadsdelsnamnd))
            {
                _logger.LogInformation("Filtering by Stadsdelsnamnd: {Stadsdelsnamnd}", Stadsdelsnamnd);
                query = query.Where(f => f.Stadsdelsnamnd == Stadsdelsnamnd);
            }

            if (!string.IsNullOrEmpty(Forskoleenhet))
            {
                _logger.LogInformation("Filtering by Forskoleenhet: {Forskoleenhet}", Forskoleenhet);
                query = query.Where(f => f.Forskoleenhet == Forskoleenhet);
            }

            if (!string.IsNullOrEmpty(SpecificQuestion))
            {
                _logger.LogInformation("Filtering by SpecificQuestion: {SpecificQuestion}", SpecificQuestion);
                query = query.Where(f => f.Fragetext.Trim() == SpecificQuestion.Trim());
            }

            var filteredQuery = query.Where(f => !ExclusionWords.Any(word => f.SvarsalternativText.Contains(word)));

            TotalPages = (int)Math.Ceiling((double)await filteredQuery.CountAsync() / pageSize);

            ForskolaFragaList = await filteredQuery
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            _logger.LogInformation("Total items found: {TotalItems}", ForskolaFragaList.Count);

            PageIndex = pageIndex;


            if (!string.IsNullOrEmpty(SpecificQuestion))
            {
                var specificQuery = filteredQuery.Where(f => f.Fragetext.Trim() == SpecificQuestion.Trim());
                int totalCount = await specificQuery.CountAsync();

                _logger.LogInformation("Total count for SpecificQuestion: {TotalCount}", totalCount);

                var responseCounts = await specificQuery
                    .GroupBy(f => f.SvarsalternativText)
                    .Select(g => new
                    {
                        Response = g.Key,
                        Count = g.Count()
                    })
                    .ToListAsync();

                ResponsePercentages = responseCounts
                    .Select(rc => new ResponsePercentage
                    {
                        Response = rc.Response,
                        Count = rc.Count,
                        Percentage = (double)rc.Count / totalCount * 100
                    })
                    .ToList();

                foreach (var response in ResponsePercentages)
                {
                    _logger.LogInformation("Response: {Response}, Count: {Count}, Percentage: {Percentage}", response.Response, response.Count, response.Percentage);
                }
            }
            else
            {
                ResponsePercentages = new List<ResponsePercentage>();
            }
        }

        public string GenerateOptionTag(string value, string currentValue)
        {
            var selected = value == currentValue ? "selected" : string.Empty;
            return $"<option value=\"{value}\" {selected}>{value}</option>";
        }
    }

    public class ResponsePercentage
    {
        public string Response { get; set; }
        public int Count { get; set; }
        public double Percentage { get; set; }
    }
}