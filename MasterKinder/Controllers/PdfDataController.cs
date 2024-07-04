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
    public class PdfDataController : ControllerBase
    {
        private readonly MrDb _context;

        public PdfDataController(MrDb context)
        {
            _context = context;
        }

        // GET: api/PdfData
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PdfData>>> GetPdfData()
        {
            return await _context.PdfData.ToListAsync();
        }


        [HttpGet("normalized-name/{name}")]
        public async Task<IActionResult> GetPdfDataByNormalizedName(string name)
        {
            var normalizedName = name.ToLower();
            var pdfData = await _context.PdfData
                .Where(p => p.NormalizedNamn == normalizedName)
                .ToListAsync();

            if (pdfData == null || !pdfData.Any())
            {
                return NotFound();
            }

            return Ok(pdfData);
        }




        // GET: api/PdfData/5
        [HttpGet("{id}")]
        public async Task<ActionResult<PdfData>> GetPdfData(int id)
        {
            var pdfData = await _context.PdfData.FindAsync(id);

            if (pdfData == null)
            {
                return NotFound();
            }

            return pdfData;
        }

        // GET: api/PdfData/name/{name}
        [HttpGet("name/{name}")]
        public async Task<ActionResult<IEnumerable<PdfData>>> GetPdfDataByName(string name)
        {
            var pdfData = await _context.PdfData
                .Where(p => p.Namn.Contains(name))
                .ToListAsync();

            if (pdfData == null || pdfData.Count == 0)
            {
                return NotFound();
            }

            return pdfData;
        }

        // POST: api/PdfData
        [HttpPost]
        public async Task<ActionResult<PdfData>> PostPdfData(PdfData pdfData)
        {
            _context.PdfData.Add(pdfData);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetPdfData", new { id = pdfData.Id }, pdfData);
        }

        // PUT: api/PdfData/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPdfData(int id, PdfData pdfData)
        {
            if (id != pdfData.Id)
            {
                return BadRequest();
            }

            _context.Entry(pdfData).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PdfDataExists(id))
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

        // DELETE: api/PdfData/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePdfData(int id)
        {
            var pdfData = await _context.PdfData.FindAsync(id);
            if (pdfData == null)
            {
                return NotFound();
            }

            _context.PdfData.Remove(pdfData);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool PdfDataExists(int id)
        {
            return _context.PdfData.Any(e => e.Id == id);
        }
    }
}
