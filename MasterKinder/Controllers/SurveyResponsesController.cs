using MasterKinder.Data;
using Microsoft.AspNetCore.Mvc;

namespace MasterKinder.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SurveyResponsesController : ControllerBase
    {
        private readonly MrDb _context;

        public SurveyResponsesController(MrDb context)
        {
            _context = context;
        }

        [HttpGet("GetResponsesByQuestion/{year}/{preschool}")]
        public IActionResult GetResponsesByQuestion(string year, string preschool, string question)
        {
            var responses = _context.SurveyResponses
                .Where(r => r.AvserAr == year && r.Forskoleenhet == preschool && r.Fragetext == question)
                .ToList();

            if (responses.Count == 0)
                return NotFound("No data available for the given criteria.");

            var totalResponses = responses.Count;
            var groupedResponses = responses.GroupBy(r => r.SvarsalternativText)
                .Select(g => new
                {
                    Answer = g.Key,
                    Count = g.Count(),
                    Percentage = (g.Count() * 100.0) / totalResponses
                })
                .ToList();

            return Ok(groupedResponses);
        }
    }
}
