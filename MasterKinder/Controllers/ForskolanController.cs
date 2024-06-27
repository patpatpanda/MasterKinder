using Microsoft.AspNetCore.Mvc;
using MasterKinder.Data;
using MasterKinder.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MasterKinder.Services;

namespace MasterKinder.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ForskolanController : ControllerBase
    {
        private readonly MrDb _context;
        private readonly GeocodeService _geocodeService;

        public ForskolanController(MrDb context, GeocodeService geocodeService)
        {
            _context = context;
            _geocodeService = geocodeService;
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
        // GET: api/Forskolan/address/{address}
        [HttpGet("address/{address}")]
        public async Task<ActionResult<IEnumerable<Forskolan>>> GetForskolansByAddress(string address)
        {
            var forskolans = await _context.Forskolans.Include(f => f.Kontakter)
                .Where(f => f.Adress.Contains(address))
                .ToListAsync();

            if (forskolans == null || forskolans.Count == 0)
            {
                return NotFound("No preschools found for the given address.");
            }

            return Ok(forskolans);
        }


        // POST: api/Forskolan
        [HttpPost]
        public async Task<ActionResult<Forskolan>> PostForskolan(Forskolan forskolan)
        {
            _context.Forskolans.Add(forskolan);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetForskolan", new { id = forskolan.Id }, forskolan);
        }
        [HttpGet("geocode/{address}")]
        public async Task<ActionResult<GeocodeResult>> GeocodeAddress(string address)
        {
            var coordinates = await _geocodeService.GeocodeAddress(address);
            if (coordinates == null)
            {
                return NotFound();
            }

            return Ok(coordinates);
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
        [HttpGet("nearby/{lat}/{lng}")]
        public async Task<ActionResult<IEnumerable<Forskolan>>> GetNearbyForskolans(double lat, double lng)
        {
            var forskolans = await _context.Forskolans.ToListAsync();

            var sortedForskolans = forskolans
                .Select(f => new
                {
                    Forskolan = f,
                    Distance = GeoHelper.Haversine(lat, lng, f.Latitude, f.Longitude)
                })
                .OrderBy(f => f.Distance)
                .Take(10)
                .Select(f => f.Forskolan)
                .ToList();

            return Ok(sortedForskolans);
        }


        private bool ForskolanExists(int id)
        {
            return _context.Forskolans.Any(e => e.Id == id);
        }
        public static class GeoHelper
        {
            public static double Haversine(double lat1, double lon1, double lat2, double lon2)
            {
                const double R = 6371; // Jordens radie i kilometer
                var dLat = (lat2 - lat1) * Math.PI / 180;
                var dLon = (lon2 - lon1) * Math.PI / 180;
                var a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
                        Math.Cos(lat1 * Math.PI / 180) * Math.Cos(lat2 * Math.PI / 180) *
                        Math.Sin(dLon / 2) * Math.Sin(dLon / 2);
                var c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
                return R * c;
            }
        }




    }
}
