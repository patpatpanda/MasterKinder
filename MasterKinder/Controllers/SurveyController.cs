using MasterKinder.Data;
using MasterKinder.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;


namespace MasterKinderAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SurveyController : ControllerBase
    {
        private readonly IMemoryCache _cache;
        private readonly MrDb _context;

        public SurveyController(MrDb context, IMemoryCache memoryCache)
        {
            _context = context;
            _cache = memoryCache;
        }
        [HttpGet("Results/{year}/{forskoleverksamhet}")]
        public async Task<IActionResult> GetSurveyResults(int year, string forskoleverksamhet)
        {
            // Skapa en nyckel baserat på år och forskoleverksamhet för att identifiera cachen
            string cacheKey = $"SurveyResults_{year}_{forskoleverksamhet}";

            // Försök att få värdet från cachen
            if (_cache.TryGetValue(cacheKey, out List<object> cachedResults))
            {
                // Om det finns ett cacheat värde, returnera det direkt
                return Ok(cachedResults);
            }

            IQueryable<ISurveyResponse> query;

            switch (year)
            {
                case 2020:
                    query = _context.SurveyResponses;
                    break;
                case 2021:
                    query = _context.SurveyResponses2021;
                    break;
                case 2022:
                    query = _context.SurveyResponses2022;
                    break;
                case 2023:
                    query = _context.SurveyResponses2023;
                    break;
                default:
                    return BadRequest("Ogiltigt år.");
            }

            string normalizedForskoleverksamhet = NormalizeName(forskoleverksamhet);

            var fragetexter = new List<string>
    {
        "Jag är som helhet nöjd med mitt barns förskola",
        "Jag kan rekommendera mitt barns förskola",
        "Jag upplever att personalen på förskolan bemöter mig på ett respektfullt sätt",
        "Jag upplever att personalen på förskolan bemöter mitt barn på ett respektfullt sätt",
        "Jag känner mig välkommen att ställa frågor och framföra synpunkter på verksamheten",
        "Mitt barns utveckling och lärande dokumenteras och synliggörs",
        "Jag upplever att jag på ett enkelt sätt kan kommunicera digitalt med mitt barns förskola",
        "Jag upplever att förskolan i sin helhet är trygg och säker för mitt barn",
        "Jag upplever att mitt barn ges möjlighet att använda digitala verktyg i sitt lärande"
    };

            var relevantResponses = await query
                .Where(r => fragetexter.Contains(r.Fragetext)
                            && (EF.Functions.Like(r.Forskoleverksamhet, $"%{forskoleverksamhet}%") || EF.Functions.Like(r.Forskoleverksamhet, $"%{normalizedForskoleverksamhet}%")))
                .GroupBy(r => new { r.Forskoleverksamhet, r.Fragetext })
                .Select(g => new
                {
                    Forskoleverksamhet = g.Key.Forskoleverksamhet,
                    Fragetext = g.Key.Fragetext,
                    TotalSvar = g.Sum(x => (x.SvarsalternativNr >= 1 && x.SvarsalternativNr <= 5) ? x.Utfall : 0),
                    ProcentSvarAlternativ = g.GroupBy(r => r.SvarsalternativNr)
                                             .Select(sg => new
                                             {
                                                 Svarsalternativ = sg.Key,
                                                 Procent = g.Sum(y => y.Utfall) == 0 ? 0 : (double)sg.Sum(x => x.Utfall) / (double)g.Sum(y => y.Utfall) * 100
                                             })
                                             .OrderBy(sg => sg.Svarsalternativ)
                                             .ToList()
                })
                .ToListAsync();

            // Cacha resultaten i 5 minuter
            _cache.Set(cacheKey, relevantResponses, TimeSpan.FromMinutes(5));

            return Ok(relevantResponses);
        }




        private string NormalizeName(string name)
        {
            if (string.IsNullOrWhiteSpace(name)) return name;

            string[] prefixes = { "Förskolan", "Föräldrakooperativet", "Föräldrakooperativ", "Daghemmet", "Daghem", "Barnstugan", "Montessoriförskolan", "Montessori", "Engelsk-svenska" };
            foreach (var prefix in prefixes)
            {
                if (name.StartsWith(prefix, System.StringComparison.OrdinalIgnoreCase))
                {
                    name = name.Substring(prefix.Length).Trim();
                }
            }

            return name.ToLower().Trim().Replace(" ", "");
        }
       
     


    }
}