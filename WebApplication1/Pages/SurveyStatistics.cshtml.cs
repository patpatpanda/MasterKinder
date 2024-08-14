using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using MasterKinder.Models;
using MasterKinder.Data;

namespace MasterKinder.Pages
{
    public class SurveyStatisticsModel : PageModel
    {
        private readonly MrDb _context;

        public SurveyStatisticsModel(MrDb context)
        {
            _context = context;
        }

        [BindProperty]
        public string Forskoleverksamhet { get; set; }

        [BindProperty]
        public string Fragetext { get; set; }

        public int AntalN�jdaSvar { get; set; }

        public async Task<IActionResult> OnGetAsync(string forskoleverksamhet, string fragetext)
        {
            if (string.IsNullOrEmpty(forskoleverksamhet) || string.IsNullOrEmpty(fragetext))
            {
                return Page(); // Returnera en tom sida om parametrarna saknas
            }

            var responses = await _context.SurveyResponses
                .Where(s => s.Forskoleverksamhet == forskoleverksamhet &&
                            s.Fragetext == fragetext &&
                            s.GraderingSvarsalternativ == "N�jd")
                .AsNoTracking() // L�gg till detta f�r att f�rb�ttra prestanda om du inte beh�ver sp�ra enheterna
                .ToListAsync();

            // Anv�nd klient-sidans utv�rdering f�r att summera resultaten
            AntalN�jdaSvar = responses
                .Where(s => s.Utfall != null)
                .Sum(s => int.Parse(s.Utfall));

            return Page();
        }

    }
}
