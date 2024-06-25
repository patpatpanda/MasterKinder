using Microsoft.AspNetCore.Mvc;
using MasterKinder.Data;
using MasterKinder.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MasterKinder.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ForskolanController : ControllerBase
    {
        private readonly MrDb _context;

        public ForskolanController(MrDb context)
        {
            _context = context;
        }

        // GET: api/Forskolan
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Forskolan>>> GetForskolans()
        {
            return await _context.Forskolans.Include(f => f.Kontakter).ToListAsync();
        }

        // GET: api/Forskolan/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<Forskolan>> GetForskolan(int id)
        {
            var forskolan = await _context.Forskolans.Include(f => f.Kontakter).FirstOrDefaultAsync(f => f.Id == id);

            if (forskolan == null)
            {
                return NotFound();
            }

            return forskolan;
        }

        // GET: api/Forskolan/address/{address}
        [HttpGet("address/{address}")]
        public async Task<ActionResult<IEnumerable<Forskolan>>> GetForskolansByAddress(string address)
        {
            var forskolans = await _context.Forskolans.Include(f => f.Kontakter)
                .Where(f => f.Adress.Contains(address))
                .ToListAsync();

            if (forskolans == null || forskolans.Count == 0)
            {
                return NotFound();
            }

            return forskolans;
        }

        // POST: api/Forskolan
        [HttpPost]
        public async Task<ActionResult<Forskolan>> PostForskolan(Forskolan forskolan)
        {
            _context.Forskolans.Add(forskolan);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetForskolan", new { id = forskolan.Id }, forskolan);
        }

        // PUT: api/Forskolan/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> PutForskolan(int id, Forskolan forskolan)
        {
            if (id != forskolan.Id)
            {
                return BadRequest();
            }

            _context.Entry(forskolan).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ForskolanExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // DELETE: api/Forskolan/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteForskolan(int id)
        {
            var forskolan = await _context.Forskolans.FindAsync(id);
            if (forskolan == null)
            {
                return NotFound();
            }

            _context.Forskolans.Remove(forskolan);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ForskolanExists(int id)
        {
            return _context.Forskolans.Any(e => e.Id == id);
        }
    }
}
