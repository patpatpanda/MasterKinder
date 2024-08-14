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

        public int AntalNöjdaSvar { get; set; }

        public async Task<IActionResult> OnGetAsync(string forskoleverksamhet, string fragetext)
        {
            if (string.IsNullOrEmpty(forskoleverksamhet) || string.IsNullOrEmpty(fragetext))
            {
                return Page(); // Returnera en tom sida om parametrarna saknas
            }

            var responses = await _context.SurveyResponses
                .Where(s => s.Forskoleverksamhet == forskoleverksamhet &&
                            s.Fragetext == fragetext &&
                            s.GraderingSvarsalternativ == "Nöjd")
                .AsNoTracking() // Lägg till detta för att förbättra prestanda om du inte behöver spåra enheterna
                .ToListAsync();

            // Använd klient-sidans utvärdering för att summera resultaten
            AntalNöjdaSvar = responses
                .Where(s => s.Utfall != null)
                .Sum(s => int.Parse(s.Utfall));

            return Page();
        }

    }
}
