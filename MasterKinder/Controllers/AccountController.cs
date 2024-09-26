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
                    Console.WriteLine("ModelState är ogiltigt.");
                    return BadRequest(ModelState);
                }

                Console.WriteLine($"Registrerar användare med Email: {model.Email} och SchoolId: {model.SchoolId}");

                var user = new ApplicationUser
                {
                    UserName = model.Email,
                    Email = model.Email,
                    SchoolId = model.SchoolId
                };

                var result = await _userManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    Console.WriteLine("Användare skapad med ID: " + user.Id);
                    await _signInManager.SignInAsync(user, isPersistent: false);
                    return Ok(new { userId = user.Id });
                }

                foreach (var error in result.Errors)
                {
                    Console.WriteLine($"Fel vid användarskapande: {error.Description}");
                    ModelState.AddModelError(string.Empty, error.Description);
                }

                return BadRequest(ModelState);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Ett undantag inträffade: " + ex.Message);
                return StatusCode(500, "Internt serverfel vid registrering.");
            }
        }



        [HttpPut("update-school/{id}")]
      
        public async Task<IActionResult> UpdateSchool(int id, [FromBody] ForskolanUpdateModel model)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return Unauthorized("Inloggad användare hittades inte.");
            }

            if (user.SchoolId != id)
            {
                return Forbid("Du har inte behörighet att uppdatera denna förskola.");
            }

            var school = await _context.Forskolans.FindAsync(id);
            if (school == null)
            {
                return NotFound("Förskolan hittades inte.");
            }

            // Uppdatera förskolans beskrivning
            school.Beskrivning = model.Beskrivning;

            await _context.SaveChangesAsync();

            return Ok(school);
        }


    }

    public class LoginModel
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }

    public class RegisterModel
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public int SchoolId { get; set; } // Lägg till SchoolId i registreringsmodellen
    }

    public class ForskolanUpdateModel
    {
       
        public string Beskrivning { get; set; }
        // Andra fält...
    }
}
