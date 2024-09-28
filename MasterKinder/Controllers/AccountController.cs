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

            // Verifiera JWT-tokenen manuellt
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
                    ClockSkew = TimeSpan.Zero // Optional: reduce the tolerance on the token expiry time
                }, out SecurityToken validatedToken);

                var jwtToken = (JwtSecurityToken)validatedToken;
                var userId = jwtToken.Claims.First(x => x.Type == "sub").Value;

                // Hämta användaren med userId
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

                logs.Add($"Uppdaterar förskolans beskrivning från '{school.Beskrivning}' till '{model.Beskrivning}'.");

                school.Beskrivning = model.Beskrivning;
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

            public string? Beskrivning { get; set; }
            // Andra fält...
        }
    }
}