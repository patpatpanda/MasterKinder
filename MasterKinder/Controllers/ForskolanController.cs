using Microsoft.AspNetCore.Mvc;
using MasterKinder.Data;
using MasterKinder.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MasterKinder.Services;
using static MasterKinder.Services.GeocodeService;

namespace MasterKinder.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ForskolanController : ControllerBase
    {
        private readonly MrDb _context;
        private readonly GeocodeService _geocodeService;
        private readonly ILogger<ForskolanController> _logger;

        public ForskolanController(MrDb context, GeocodeService geocodeService, ILogger<ForskolanController> logger)
        {
            _context = context;
            _geocodeService = geocodeService;
            _logger = logger;
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
            try
            {
                var coordinates = await _geocodeService.GeocodeAddress(address);
                if (coordinates == null)
                {
                    return NotFound("Geocoding failed.");
                }

                return Ok(coordinates);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error in geocoding address: {address}");
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
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
        public async Task<ActionResult<IEnumerable<Forskolan>>> GetNearbyForskolans(
      double lat, double lng,
      [FromQuery] string organisationsform = "alla",
      [FromQuery] string typAvService = "alla")
        {
            var forskolans = await _context.Forskolans.ToListAsync();

            // Filtrera baserat på organisationsform
            if (!string.IsNullOrEmpty(organisationsform) && organisationsform.ToLower() != "alla")
            {
                var orgForms = organisationsform.Split(',').Select(o => o.Trim()).ToList();
                forskolans = forskolans.Where(f => orgForms.Contains(f.Organisationsform, StringComparer.OrdinalIgnoreCase)).ToList();
            }

            // Filtrera baserat på typ av service
            if (!string.IsNullOrEmpty(typAvService) && typAvService.ToLower() != "alla")
            {
                forskolans = forskolans.Where(f => f.TypAvService.Equals(typAvService, StringComparison.OrdinalIgnoreCase)).ToList();
            }

            // Beräkna avstånd och sortera
            var sortedForskolans = forskolans
                .Select(f => new
                {
                    Forskolan = f,
                    Distance = GeoHelper.Haversine(lat, lng, f.Latitude, f.Longitude)
                })
                .OrderBy(f => f.Distance)
                .Take(15)
                .Select(f => f.Forskolan)
                .ToList();

            return Ok(sortedForskolans);
        }




        [HttpGet("walking-time")]
        public ActionResult<double> GetWalkingTime(double lat1, double lon1, double lat2, double lon2)
        {
            const double walkingSpeedKmPerHour = 5.0; // Antag konstant gånghastighet på 5 km/h
            var distanceInKm = GeoHelper.Haversine(lat1, lon1, lat2, lon2);
            var timeInHours = distanceInKm / walkingSpeedKmPerHour;
            return Ok(timeInHours);
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