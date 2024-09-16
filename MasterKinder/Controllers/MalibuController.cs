using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MasterKinder.Data;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MasterKinder.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MalibuController : ControllerBase
    {
        private readonly MrDb _context;

        public MalibuController(MrDb context)
        {
            _context = context;
        }

        [HttpGet("name/{name}")]
        public async Task<ActionResult<IEnumerable<Malibu>>> GetMalibuByName(string name)
        {
            string normalizedSearchName = NormalizeName(name);
            string[] searchWords = name.Split(' ');

            // Försök att hitta genom normaliserade namn
            var malibus = await _context.Malibus
                .Include(m => m.Questions)
                .Where(m => EF.Functions.Like(m.Namn, $"%{name}%") || EF.Functions.Like(m.NormalizedNamn, $"%{normalizedSearchName}%"))
                .ToListAsync();

            // Om inga träffar, sök baserat på ord
            if (!malibus.Any())
            {
                var query = _context.Malibus.Include(m => m.Questions).AsQueryable();
                foreach (var word in searchWords)
                {
                    string normalizedWord = NormalizeName(word);
                    query = query.Where(m => EF.Functions.Like(m.Namn, $"%{word}%") || EF.Functions.Like(m.NormalizedNamn, $"%{normalizedWord}%"));
                }
                malibus = await query.ToListAsync();
            }

            if (!malibus.Any())
            {
                return NotFound();
            }

            return Ok(malibus);
        }

        private string NormalizeName(string name)
        {
            if (string.IsNullOrWhiteSpace(name)) return name;

            // Ta bort vanliga prefix och trimma resultatet
            string[] prefixes = { "Förskolan", "Föräldrakooperativet", "Föräldrakooperativ", "Daghemmet", "Daghem", "Barnstugan", "Montessoriförskolan", "Montessori", "Engelsk-svenska" };
            foreach (var prefix in prefixes)
            {
                if (name.StartsWith(prefix, System.StringComparison.OrdinalIgnoreCase))
                {
                    name = name.Substring(prefix.Length).Trim();
                }
            }

            // Hantera specialtecken som tankstreck och ersätt dem med mellanslag
            name = name.Replace("–", " ").Replace("-", " ");

            // Sortera orden för att hantera olika ordning på namn
            var words = name.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            Array.Sort(words);

            // Gör namnet till gemener och ta bort eventuella extra mellanslag
            return string.Join(" ", words).ToLower().Trim().Replace(" ", "");
        }

    }
}