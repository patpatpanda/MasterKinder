using MasterKinder.Data;
using MasterKinder.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace MasterKinderAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SurveyController : ControllerBase
    {
        private readonly MrDb _context;

        public SurveyController(MrDb context)
        {
            _context = context;
        }
        [HttpGet("Results/{year}/{forskoleverksamhet}")]
        public async Task<IActionResult> GetSurveyResults(int year, string forskoleverksamhet)
        {
            IQueryable<ISurveyResponse> query;

            // Välj rätt tabell beroende på år
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

            // Normalisera namnet för att använda det i sökningen
            string normalizedForskoleverksamhet = NormalizeName(forskoleverksamhet);

            // Filtrera på frågetext och normaliserat forskoleverksamhet
            var relevantResponses = await query
                .Where(r => r.Fragetext == "Jag är som helhet nöjd med mitt barns förskola"
                            && (EF.Functions.Like(r.Forskoleverksamhet, $"%{forskoleverksamhet}%") || EF.Functions.Like(r.Forskoleverksamhet, $"%{normalizedForskoleverksamhet}%")))
                .GroupBy(r => r.Forskoleverksamhet)
                .Select(g => new
                {
                    Forskoleverksamhet = g.Key,
                    TotalSvar = g.Sum(x => (x.SvarsalternativNr >= 1 && x.SvarsalternativNr <= 5) ? x.Utfall : 0), // Korrekt beräkning av TotalSvar
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

            return Ok(relevantResponses);
        }


        // GET: api/Survey?year=2020&forskoleverksamhet=XYZ&fragetext=ABC
        [HttpGet]
        public async Task<IActionResult> GetSurveyResponses(int year, string forskoleverksamhet, string fragetext)
        {
            if (string.IsNullOrEmpty(forskoleverksamhet) || string.IsNullOrEmpty(fragetext))
            {
                return BadRequest("Förskoleverksamhet och Frågetext måste anges.");
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

            var responses = await query
                .Where(s => s.Forskoleverksamhet == forskoleverksamhet && s.Fragetext == fragetext)
                .ToListAsync();

            return Ok(responses);
        }

        [HttpGet("nojd")]
        public async Task<IActionResult> GetNöjdResponses(int year, string forskoleverksamhet, string fragetext = null, string frageNr = null)
        {
            if (string.IsNullOrEmpty(forskoleverksamhet))
            {
                return BadRequest("Förskoleverksamhet måste anges.");
            }

            if (string.IsNullOrEmpty(fragetext) && string.IsNullOrEmpty(frageNr))
            {
                return BadRequest("Antingen Frågetext eller FrageNr måste anges.");
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

            // Hämta alla svar för den specifika frågan vid den angivna förskoleverksamheten
            query = query.Where(s => s.Forskoleverksamhet == forskoleverksamhet);

            if (!string.IsNullOrEmpty(fragetext))
            {
                query = query.Where(s => s.Fragetext == fragetext);
            }

            if (!string.IsNullOrEmpty(frageNr))
            {
                query = query.Where(s => s.FrageNr == frageNr);
            }

            var responses = await query.ToListAsync();

            // Räkna antalet nöjda svar
            var antalNöjdaSvar = responses
     .Where(s => s.SvarsalternativText == "3" ||
                 s.SvarsalternativText == "4" ||
                 s.SvarsalternativText == "5" ||
                 s.SvarsalternativText == "Instämmer" ||
                 s.SvarsalternativText == "Instämmer i stor utsträckning" ||
                 s.SvarsalternativText == "Instämmer helt")
     .Sum(s => s.Utfall);  // Använd Utfall direkt eftersom det är en int


            // Räkna det totala antalet svar för frågan
            var totaltAntalSvar = responses.Sum(s => s.Utfall);  // Använd Utfall direkt eftersom det är en int


            // Returnera både antalet nöjda svar och det totala antalet svar
            return Ok(new
            {
                AntalNöjdaSvar = antalNöjdaSvar,
                TotaltAntalSvar = totaltAntalSvar
            });
        }
        [HttpGet("svarsalternativ")]
        public async Task<IActionResult> GetSvarsalternativResponses(int year, string forskoleverksamhet, string fragetext = null, string frageNr = null)
        {
            Console.WriteLine($"Received: year={year}, forskoleverksamhet={forskoleverksamhet}, fragetext={fragetext}, frageNr={frageNr}");

            if (string.IsNullOrEmpty(forskoleverksamhet))
            {
                return BadRequest("Förskoleverksamhet måste anges.");
            }

            if (string.IsNullOrEmpty(fragetext) && string.IsNullOrEmpty(frageNr))
            {
                return BadRequest("Antingen Frågetext eller FrageNr måste anges.");
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

            query = query.Where(s => s.Forskoleverksamhet == forskoleverksamhet);

            if (!string.IsNullOrEmpty(fragetext))
            {
                query = query.Where(s => s.Fragetext == fragetext);
            }

            if (!string.IsNullOrEmpty(frageNr))
            {
                query = query.Where(s => s.FrageNr == frageNr);
            }

            var responses = await query.ToListAsync();

            if (!responses.Any())
            {
                return Ok(new List<object>());
            }

            var aggregatedData = responses
                .GroupBy(s => s.SvarsalternativText)
                .Select(g => new
                {
                    SvarsalternativText = g.Key,
                    Utfall = g.Sum(s => s.Utfall) // Använd Utfall direkt eftersom det redan är en int

        })
                .ToList();

            return Ok(aggregatedData);
        }

        // GET: api/Survey/forskoleverksamheter
        [HttpGet("forskoleverksamheter")]
        public async Task<IActionResult> GetForskoleverksamheter(int year)
        {
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

            var forskoleverksamheter = await query
                .Select(s => s.Forskoleverksamhet)
                .Distinct()
                .ToListAsync();

            return Ok(forskoleverksamheter);
        }

        // GET: api/Survey/fragetexter
        [HttpGet("fragetexter")]
        public async Task<IActionResult> GetFragetexter(int year)
        {
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

            var fragetexter = await query
                .Select(s => s.Fragetext)
                .Distinct()
                .ToListAsync();

            return Ok(fragetexter);
        }

        [HttpGet("survey/name/{name}")]
        public async Task<ActionResult<IEnumerable<SurveyResponse>>> GetSurveyResponseByName(string name)
        {
            string normalizedSearchName = NormalizeName(name);
            string[] searchWords = name.Split(' ');

            var surveyResponses = await _context.SurveyResponses
                .Where(s => EF.Functions.Like(NormalizeName(s.Forskoleverksamhet), $"%{normalizedSearchName}%"))
                .ToListAsync();

            if (!surveyResponses.Any())
            {
                var query = _context.SurveyResponses.AsQueryable();
                foreach (var word in searchWords)
                {
                    string normalizedWord = NormalizeName(word);
                    query = query.Where(s => EF.Functions.Like(NormalizeName(s.Forskoleverksamhet), $"%{normalizedWord}%"));
                }
                surveyResponses = await query.ToListAsync();
            }

            if (!surveyResponses.Any())
            {
                return NotFound();
            }

            return Ok(surveyResponses);
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
        [HttpGet("satisfaction-summary")]
        public async Task<ActionResult<IEnumerable<SatisfactionSummary>>> GetSatisfactionSummary()
        {
            var satisfactionData = await _context.SurveyResponses
                .Where(s => s.Fragetext == "Jag är som helhet nöjd med mitt barns förskola")
                .GroupBy(s => new { s.Forskoleverksamhet, s.AvserAr })
                .Select(g => new SatisfactionSummary
                {
                    Forskoleverksamhet = g.Key.Forskoleverksamhet,
                    Year = g.Key.AvserAr,
                    TotalResponses = g.Count(),
                    PositiveResponses = g.Count(s => s.SvarsalternativText == "Instämmer helt" || s.SvarsalternativText == "Instämmer i stor utsträckning")
                })
                .ToListAsync();

            return Ok(satisfactionData);
        }
        [HttpGet("satisfaction-summary/name/{name}")]
        public async Task<ActionResult<IEnumerable<SatisfactionSummary>>> GetSatisfactionSummaryByName(string name)
        {
            string normalizedSearchName = NormalizeName(name);
            string[] searchWords = name.Split(' ');

            // Första sökning: försök att hitta baserat på Namn och NormalizedNamn
            var satisfactionData = await _context.SurveyResponses
                .Where(s => s.Fragetext == "Jag är som helhet nöjd med mitt barns förskola")
                .Where(s => EF.Functions.Like(s.Forskoleverksamhet, $"%{name}%") || EF.Functions.Like(s.Forskoleverksamhet, $"%{normalizedSearchName}%"))
                .GroupBy(s => new { s.Forskoleverksamhet, s.AvserAr })
                .Select(g => new SatisfactionSummary
                {
                    Forskoleverksamhet = g.Key.Forskoleverksamhet,
                    Year = g.Key.AvserAr,
                    TotalResponses = g.Count(),
                    PositiveResponses = g.Count(s => s.SvarsalternativText == "Instämmer helt" || s.SvarsalternativText == "Instämmer i stor utsträckning")
                })
                .ToListAsync();

            // Om inga träffar, försök att söka på varje ord individuellt
            if (!satisfactionData.Any())
            {
                var query = _context.SurveyResponses
                    .Where(s => s.Fragetext == "Jag är som helhet nöjd med mitt barns förskola")
                    .AsQueryable();

                foreach (var word in searchWords)
                {
                    string normalizedWord = NormalizeName(word);
                    query = query.Where(s => EF.Functions.Like(s.Forskoleverksamhet, $"%{word}%") || EF.Functions.Like(s.Forskoleverksamhet, $"%{normalizedWord}%"));
                }

                satisfactionData = await query
                    .GroupBy(s => new { s.Forskoleverksamhet, s.AvserAr })
                    .Select(g => new SatisfactionSummary
                    {
                        Forskoleverksamhet = g.Key.Forskoleverksamhet,
                        Year = g.Key.AvserAr,
                        TotalResponses = g.Count(),
                        PositiveResponses = g.Count(s => s.SvarsalternativText == "Instämmer helt" || s.SvarsalternativText == "Instämmer i stor utsträckning")
                    })
                    .ToListAsync();
            }

            if (!satisfactionData.Any())
            {
                return NotFound();
            }

            return Ok(satisfactionData);
        }
        public class SatisfactionSummary
        {
            public string Forskoleverksamhet { get; set; }
            public string Year { get; set; }
            public int TotalResponses { get; set; }
            public int PositiveResponses { get; set; }
        }


    }
}