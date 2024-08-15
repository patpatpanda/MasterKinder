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
                .Sum(s => string.IsNullOrEmpty(s.Utfall) ? 0 : int.TryParse(s.Utfall, out var value) ? value : 0);

            // Räkna det totala antalet svar för frågan
            var totaltAntalSvar = responses
                .Sum(s => string.IsNullOrEmpty(s.Utfall) ? 0 : int.TryParse(s.Utfall, out var value) ? value : 0);

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

            // Hämta datan som rådata
            var responses = await query.ToListAsync();

            // Gruppera och summera resultaten i minnet
            var aggregatedData = responses
                .GroupBy(s => s.SvarsalternativText)
                .Select(g => new
                {
                    SvarsalternativText = g.Key,
                    Utfall = g.Sum(s => int.TryParse(s.Utfall, out var value) ? value : 0)
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


    }
}