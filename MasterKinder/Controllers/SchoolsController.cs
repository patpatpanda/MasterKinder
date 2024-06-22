using Microsoft.AspNetCore.Mvc;
using MasterKinder.Services;
using MasterKinder.Models;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace MasterKinder.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SchoolsController : ControllerBase
    {
        private readonly ISchoolService _schoolService;
        private readonly ILogger<SchoolsController> _logger;

        public SchoolsController(ISchoolService schoolService, ILogger<SchoolsController> logger)
        {
            _schoolService = schoolService;
            _logger = logger;
        }

        // POST: api/schools
        [HttpPost]
        public async Task<IActionResult> PostSchool([FromBody] CreateSchoolRequest request)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogError("Invalid model state: {ModelState}", ModelState);
                foreach (var state in ModelState)
                {
                    foreach (var error in state.Value.Errors)
                    {
                        _logger.LogError("Error in field {Field}: {ErrorMessage}", state.Key, error.ErrorMessage);
                    }
                }
                return BadRequest(ModelState);
            }

            var school = new School
            {
                SchoolName = request.SchoolName,
                TotalResponses = request.TotalResponses,
                SatisfactionPercentage = request.SatisfactionPercentage,
                Responses = new List<Response>()
            };

            foreach (var responseRequest in request.Responses)
            {
                school.Responses.Add(new Response
                {
                    Question = responseRequest.Question,
                    Percentage = responseRequest.Percentage,
                    Gender = responseRequest.Gender,
                    Year = responseRequest.Year,
                    Category = responseRequest.Category // Lägg till detta fält
                });
            }

            await _schoolService.AddSchool(school);

            return CreatedAtAction(nameof(GetSchoolById), new { id = school.SchoolId }, school);
        }
        [HttpGet("details/{id}")]
        public async Task<IActionResult> GetSchoolDetails(int id)
        {
            var school = await _schoolService.GetSchoolById(id);

            if (school == null)
            {
                return NotFound();
            }

            var helhetsomdome = school.Responses.FirstOrDefault(r => r.Question == "Helhetsomdöme")?.Percentage ?? 0;
            var totalResponses = school.TotalResponses;
            var svarsfrekvens = school.SatisfactionPercentage; // Assuming this represents response frequency
            var antalBarn = totalResponses > 0 ? (int)(totalResponses / (svarsfrekvens / 100)) : 0;

            return Ok(new
            {
                SchoolName = school.SchoolName,
                Helhetsomdome = helhetsomdome,
                TotalResponses = totalResponses,
                Svarsfrekvens = svarsfrekvens,
                AntalBarn = antalBarn
            });
        }

        [HttpGet("details/google/{placeName}")]
        public async Task<IActionResult> GetSchoolDetailsByGoogleName(string placeName)
        {
            var schoolDetails = await _schoolService.GetSchoolDetailsByGoogleName(placeName);

            if (schoolDetails == null)
            {
                return NotFound();
            }

            return Ok(schoolDetails);
        }


        // GET: api/schools/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetSchoolById(int id)
        {
            var school = await _schoolService.GetSchoolById(id);

            if (school == null)
            {
                return NotFound();
            }

            return Ok(school);
        }
    }
}
