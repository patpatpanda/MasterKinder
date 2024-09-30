using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using MasterKinder.Data;
using MasterKinder.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Configuration;

namespace MasterKinder.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly MrDb _context; // Lägg till databaskontexten för att hantera förskoleuppdateringar
        private readonly IConfiguration _configuration;
        public AccountController(SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager, MrDb context, IConfiguration configuration)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _context = context; // Initialisera context
            _configuration = configuration;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
                return Unauthorized();

            var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, false, lockoutOnFailure: false);

            if (result.Succeeded)
            {
                // Generera JWT-token
                var token = GenerateJwtToken(user);

                // Returnera token och användarens schoolId
                return Ok(new
                {
                    token = token,
                    schoolId = user.SchoolId  // Se till att SchoolId finns i ApplicationUser-modellen
                });
            }

            return Unauthorized();
        }


        private string GenerateJwtToken(ApplicationUser user)
        {
            var claims = new[]
            {
        new Claim(JwtRegisteredClaimNames.Sub, user.Id),
        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
    };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(30),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                // Kontrollera om angivet SchoolId finns i databasen
                var schoolExists = await _context.Forskolans.AnyAsync(s => s.Id == model.SchoolId);
                if (!schoolExists)
                {
                    return BadRequest(new { message = "Ogiltigt SchoolId." });
                }

                var user = new ApplicationUser
                {
                    UserName = model.Email,
                    Email = model.Email,
                    SchoolId = model.SchoolId  // Sätt SchoolId för den nya användaren
                };

                var result = await _userManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    await _signInManager.SignInAsync(user, isPersistent: false);
                    return Ok(new { userId = user.Id });
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }

                return BadRequest(ModelState);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internt serverfel vid registrering.");
            }
        }


        [HttpPost("update-school/{id}")]
        public async Task<IActionResult> UpdateSchool(int id, [FromBody] ForskolanUpdateModel model)
        {
            List<string> logs = new List<string>();
            logs.Add($"Mottog uppdateringsförfrågan för förskola {id}.");

            var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");

            if (string.IsNullOrEmpty(token))
            {
                return Unauthorized(new { message = "Ingen token hittades." });
            }

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]);

            try
            {
                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = _configuration["Jwt:Issuer"],
                    ValidAudience = _configuration["Jwt:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ClockSkew = TimeSpan.Zero
                }, out SecurityToken validatedToken);

                var jwtToken = (JwtSecurityToken)validatedToken;
                var userId = jwtToken.Claims.First(x => x.Type == "sub").Value;

                var user = await _userManager.FindByIdAsync(userId);
                if (user == null)
                {
                    logs.Add("Användaren hittades inte.");
                    return Unauthorized(new { message = "Användaren är inte autentiserad." });
                }

                if (user.SchoolId != id)
                {
                    logs.Add($"Användare med SchoolId {user.SchoolId} försökte uppdatera skola {id} utan behörighet.");
                    return new ObjectResult(new ResponseModel("Du har inte behörighet att uppdatera denna förskola.", logs))
                    {
                        StatusCode = StatusCodes.Status403Forbidden
                    };
                }

                var school = await _context.Forskolans.FindAsync(id);
                if (school == null)
                {
                    logs.Add($"Skola med id {id} hittades inte.");
                    return new ObjectResult(new ResponseModel("Förskolan hittades inte.", logs))
                    {
                        StatusCode = StatusCodes.Status404NotFound
                    };
                }

                // Uppdatera skolans uppgifter om de är angivna i modellen
                if (!string.IsNullOrEmpty(model.Namn))
                {
                    logs.Add($"Uppdaterar namn från '{school.Namn}' till '{model.Namn}'.");
                    school.Namn = model.Namn;
                }

                if (!string.IsNullOrEmpty(model.Adress))
                {
                    logs.Add($"Uppdaterar adress från '{school.Adress}' till '{model.Adress}'.");
                    school.Adress = model.Adress;
                }

                if (!string.IsNullOrEmpty(model.Beskrivning))
                {
                    logs.Add($"Uppdaterar beskrivning från '{school.Beskrivning}' till '{model.Beskrivning}'.");
                    school.Beskrivning = model.Beskrivning;
                }

                // Fortsätt uppdatera alla fält
                school.TypAvService = model.TypAvService ?? school.TypAvService;
                school.VerksamI = model.VerksamI ?? school.VerksamI;
                school.Organisationsform = model.Organisationsform ?? school.Organisationsform;
                school.AntalBarn = model.AntalBarn ?? school.AntalBarn;
                school.AntalBarnPerArsarbetare = model.AntalBarnPerArsarbetare ?? school.AntalBarnPerArsarbetare;
                school.AndelLegitimeradeForskollarare = model.AndelLegitimeradeForskollarare ?? school.AndelLegitimeradeForskollarare;
                school.Webbplats = model.Webbplats ?? school.Webbplats;
                school.InriktningOchProfil = model.InriktningOchProfil ?? school.InriktningOchProfil;
                school.InneOchUtemiljo = model.InneOchUtemiljo ?? school.InneOchUtemiljo;
                school.KostOchMaltider = model.KostOchMaltider ?? school.KostOchMaltider;
                school.MalOchVision = model.MalOchVision ?? school.MalOchVision;
                school.MerOmOss = model.MerOmOss ?? school.MerOmOss;
                school.Latitude = model.Latitude ?? school.Latitude;
                school.Longitude = model.Longitude ?? school.Longitude;
                school.BildUrl = model.BildUrl ?? school.BildUrl;

                await _context.SaveChangesAsync();
                logs.Add("Uppdateringen genomfördes framgångsrikt.");
                return Ok(new ResponseModel("Uppdatering lyckades", logs));
            }
            catch (SecurityTokenException)
            {
                return Unauthorized(new { message = "Token är ogiltig eller har gått ut." });
            }
        }


        public class LoginModel
        {
            public string Email { get; set; }
            public string Password { get; set; }
        }
        public class ResponseModel
        {
            public string Message { get; set; }
            public List<string> Logs { get; set; }

            public ResponseModel(string message, List<string> logs)
            {
                Message = message;
                Logs = logs;
            }
        }

        public class RegisterModel
        {
            public string Email { get; set; }
            public string Password { get; set; }
            public int SchoolId { get; set; } // Lägg till SchoolId i registreringsmodellen
        }

        public class ForskolanUpdateModel
        {
            public string? Namn { get; set; }
            public string? Adress { get; set; }
            public string? Beskrivning { get; set; }
            public string? TypAvService { get; set; }
            public string? VerksamI { get; set; }
            public string? Organisationsform { get; set; }
            public int? AntalBarn { get; set; }
            public double? AntalBarnPerArsarbetare { get; set; }
            public double? AndelLegitimeradeForskollarare { get; set; }
            public string? Webbplats { get; set; }
            public string? InriktningOchProfil { get; set; }
            public string? InneOchUtemiljo { get; set; }
            public string? KostOchMaltider { get; set; }
            public string? MalOchVision { get; set; }
            public string? MerOmOss { get; set; }
            public double? Latitude { get; set; }
            public double? Longitude { get; set; }
            public string? BildUrl { get; set; }
        }

    }
}