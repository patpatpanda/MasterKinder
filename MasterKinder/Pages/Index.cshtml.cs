using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using MasterKinder.Data;
using MasterKinder.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MasterKinder.Pages
{
    public class IndexModel : PageModel
    {
        private readonly AppDbContext _context;

        public IndexModel(AppDbContext context)
        {
            _context = context;
        }

        [BindProperty(SupportsGet = true)]
        public string SearchString { get; set; }

        [BindProperty(SupportsGet = true)]
        public string Year { get; set; }

        [BindProperty(SupportsGet = true)]
        public string Stadsdelsnamnd { get; set; }

        [BindProperty(SupportsGet = true)]
        public string FragaomradeNr { get; set; }

        [BindProperty(SupportsGet = true)]
        public string Kon { get; set; }

        [BindProperty(SupportsGet = true)]
        public int PageNumber { get; set; } = 1;

        public int PageSize { get; set; } = 10;
        public int TotalPages { get; set; }

        public IList<SurveyResponse> SurveyResponses { get; set; }
        public IList<string> Years { get; set; }
        public IList<string> Districts { get; set; }
        public IList<string> QuestionAreas { get; set; }

        public async Task OnGetAsync()
        {
            var responses = _context.SurveyResponses.AsQueryable();

            if (!string.IsNullOrEmpty(SearchString))
            {
                responses = responses.Where(s => s.Forskoleenhet.Contains(SearchString) || s.Stadsdelsnamnd.Contains(SearchString));
            }

            if (!string.IsNullOrEmpty(Year))
            {
                responses = responses.Where(s => s.AvserAr == Year);
            }

            if (!string.IsNullOrEmpty(Stadsdelsnamnd))
            {
                responses = responses.Where(s => s.Stadsdelsnamnd == Stadsdelsnamnd);
            }

            if (!string.IsNullOrEmpty(FragaomradeNr))
            {
                responses = responses.Where(s => s.Fragetext == FragaomradeNr);
            }

            if (!string.IsNullOrEmpty(Kon))
            {
                responses = responses.Where(s => s.Kon == Kon);
            }

            var totalRecords = await responses.CountAsync();
            TotalPages = (int)Math.Ceiling(totalRecords / (double)PageSize);

            SurveyResponses = await responses
                .Skip((PageNumber - 1) * PageSize)
                .Take(PageSize)
                .Select(s => new SurveyResponse
                {
                    Forskoleenhet = s.Forskoleenhet,
                    Stadsdelsnamnd = s.Stadsdelsnamnd,
                    AvserAr = s.AvserAr,
                    Fragaomradestext = s.Fragaomradestext,
                    Fragetext = s.Fragetext,
                    SvarsalternativText = s.SvarsalternativText,
                    Utfall = s.Utfall
                })
                .ToListAsync();

            // Populate filter options
            Years = await _context.SurveyResponses.Select(r => r.AvserAr).Distinct().ToListAsync();
            Districts = await _context.SurveyResponses.Select(r => r.Stadsdelsnamnd).Distinct().ToListAsync();
            QuestionAreas = await _context.SurveyResponses.Select(r => r.Fragetext).Distinct().ToListAsync();
        }
    }
}
