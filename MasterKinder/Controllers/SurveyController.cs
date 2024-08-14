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

        // GET: api/Survey/nöjd?year=2020&forskoleverksamhet=XYZ&fragetext=ABC
        [HttpGet("nöjd")]
        public async Task<IActionResult> GetNöjdResponses(int year, string forskoleverksamhet, string fragetext)
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

            var antalNöjdaSvar = await query
                .Where(s => s.Forskoleverksamhet == forskoleverksamhet &&
                            s.Fragetext == fragetext &&
                            s.GraderingSvarsalternativ == "Nöjd")
                .SumAsync(s => int.Parse(s.Utfall));

            return Ok(antalNöjdaSvar);
        }
    }
}
