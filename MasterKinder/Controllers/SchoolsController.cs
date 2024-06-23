using Microsoft.AspNetCore.Mvc;
using MasterKinder.Services;
using MasterKinder.Models;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using System.Collections.Generic;

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
                SatisfactionPercentage = request.ResponseRatePercentage,
                NumberOfChildren = request.NumberOfChildren,
                Address = request.Address,
                Description = request.Description,
                Principal = request.Contact.Principal,
                Email = request.Contact.Email,
                Phone = request.Contact.Phone,
                Website = request.Contact.Website,
                TypeOfService = request.TypeOfService,
                OperatingArea = request.OperatingArea,
                OrganizationForm = request.OrganizationForm,
                ChildrenPerEmployee = request.ChildrenPerEmployee,
                PercentageOfLicensedTeachers = request.PercentageOfLicensedTeachers,
                Accessibility = request.Accessibility,
                OrientationAndProfile = request.OrientationAndProfile,
                IndoorDescription = request.IndoorDescription,
                OutdoorDescription = request.OutdoorDescription,
                FoodAndMealsDescription = request.FoodAndMealsDescription,
                GoalsAndVisionDescription = request.GoalsAndVisionDescription,
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
                    Category = responseRequest.Category,
                    School = school // Lägg till detta om det inte skapar en cykel
                });
            }

            await _schoolService.AddSchool(school);

            return CreatedAtAction(nameof(GetSchoolById), new { id = school.SchoolId }, new
            {
                school.SchoolId,
                school.SchoolName,
                school.TotalResponses,
                school.SatisfactionPercentage,
                school.NumberOfChildren,
                school.Address,
                school.Description,
                Contact = new
                {
                    school.Principal,
                    school.Email,
                    school.Phone,
                    school.Website
                },
                school.TypeOfService,
                school.OperatingArea,
                school.OrganizationForm,
                school.ChildrenPerEmployee,
                school.PercentageOfLicensedTeachers,
                school.Accessibility,
                school.OrientationAndProfile,
                school.IndoorDescription,
                school.OutdoorDescription,
                school.FoodAndMealsDescription,
                school.GoalsAndVisionDescription,
                Responses = school.Responses.Select(r => new
                {
                    r.Question,
                    r.Percentage,
                    r.Gender,
                    r.Year,
                    r.Category
                })
            });
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
            var svarsfrekvens = school.SatisfactionPercentage;
            var antalBarn = school.NumberOfChildren;

            return Ok(new
            {
                SchoolName = school.SchoolName,
                Helhetsomdome = helhetsomdome,
                TotalResponses = totalResponses,
                Svarsfrekvens = svarsfrekvens,
                AntalBarn = antalBarn,
                Address = school.Address,
                Description = school.Description,
                Contact = new
                {
                    Principal = school.Principal,
                    Email = school.Email,
                    Phone = school.Phone,
                    Website = school.Website
                },
                TypeOfService = school.TypeOfService,
                OperatingArea = school.OperatingArea,
                OrganizationForm = school.OrganizationForm,
                ChildrenPerEmployee = school.ChildrenPerEmployee,
                PercentageOfLicensedTeachers = school.PercentageOfLicensedTeachers,
                Accessibility = school.Accessibility,
                OrientationAndProfile = school.OrientationAndProfile,
                IndoorDescription = school.IndoorDescription,
                OutdoorDescription = school.OutdoorDescription,
                FoodAndMealsDescription = school.FoodAndMealsDescription,
                GoalsAndVisionDescription = school.GoalsAndVisionDescription
            });
        }

        // Ny metod för att hämta detaljer genom Google-namn
        [HttpGet("details/google/{placeAddress}")]
        public async Task<IActionResult> GetSchoolDetailsByGoogleName(string placeAddress)
        {
            var schoolDetails = await _schoolService.GetSchoolDetailsByGoogleName(placeAddress);

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

        // GET: api/schools
        [HttpGet]
        public async Task<IActionResult> GetAllSchools()
        {
            var schools = await _schoolService.GetAllSchools();
            return Ok(schools);
        }
    }
}
