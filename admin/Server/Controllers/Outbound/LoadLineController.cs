#nullable disable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Application.Server.Data;
using Application.Shared.Models.Outbound;
using Application.Shared.Models.Enum;

namespace Application.Server.Controllers.Outbound
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoadLineController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public LoadLineController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/LoadLine
        [HttpGet]
        public async Task<ActionResult<IEnumerable<LoadLine>>> GetLoadLine()
        {
            return await _context.LoadLine.ToListAsync();
        }

        [HttpGet("filter")]
        public async Task<ActionResult<IEnumerable<LoadLine>>> GetRouteLineFiltered([FromQuery] string headerId)
        {
            return await _context.LoadLine.Include(s => s.LicensePlate).ThenInclude(c => c.Store)
                                            // .Include(s => s.Consolidation).ThenInclude(c => c.LicensePlate)
                                            .Where(r => r.LoadHeaderId == headerId).ToListAsync();
        }

        // GET: api/LoadLine/5
        [HttpGet("{id}")]
        public async Task<ActionResult<LoadLine>> GetLoadLine(string id)
        {
            var loadLine = await _context.LoadLine.Include(s => s.LicensePlate).FirstOrDefaultAsync(l => l.Id == id);

            if (loadLine == null)
            {
                return NotFound();
            }

            return loadLine;
        }

        // PUT: api/LoadLine/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutLoadLine(string id, LoadLine loadLine)
        {
            if (id != loadLine.Id)
            {
                return BadRequest();
            }

            _context.Entry(loadLine).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!LoadLineExists(id))
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

        // POST: api/LoadLine
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<LoadLine>> PostLoadLine(LoadLine loadLine)
        {
            
            // Update consolidation status to loaded
            var consolidations = await _context.Consolidation.Where(c => c.LicensePlateId == loadLine.LicensePlateId).ToListAsync();

            foreach (var consolidation in consolidations)
            {
                consolidation.ConsolidationStatus = ConsolidationStatus.Loaded;
                _context.Consolidation.Update(consolidation);                
            }


            // update license plate status to loaded
            var licensePlate = await _context.LicensePlate.FindAsync(loadLine.LicensePlateId);
            licensePlate.LicensePlateStatus = LicensePlateStatus.Loaded;
            _context.LicensePlate.Update(licensePlate);

            // Add loadline
            _context.LoadLine.Add(loadLine);

            try
            {

                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (LoadLineExists(loadLine.Id))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }
            
            

            return CreatedAtAction("GetLoadLine", new { id = loadLine.Id, licensePlate = loadLine.LicensePlate }, loadLine);
        }

        // DELETE: api/LoadLine/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteLoadLine(string id)
        {
            var loadLine = await _context.LoadLine.FindAsync(id);
            loadLine.LoadStatus = LoadStatus.Loaded;

            // Update consolidation status to loaded
            var consolidations = await _context.Consolidation.Where(c => c.LicensePlateId == loadLine.LicensePlateId).ToListAsync();

            foreach (var consolidation in consolidations)
            {
                consolidation.ConsolidationStatus = ConsolidationStatus.Pending;
                _context.Consolidation.Update(consolidation);                
            }


            // update license plate status to loaded
            var licensePlate = await _context.LicensePlate.FindAsync(loadLine.LicensePlateId);
            licensePlate.LicensePlateStatus = LicensePlateStatus.Consolidated;
            _context.LicensePlate.Update(licensePlate);


            // Update Load header status to loading
            // var loadHeader = await _context.LoadHeader.FindAsync(loadLine.LoadHeaderId);
            // loadHeader.LoadStatus = LoadStatus.Loading;
            // _context.LoadHeader.Update(loadHeader);


            if (loadLine == null)
            {
                return NotFound();
            }

            _context.LoadLine.Remove(loadLine);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool LoadLineExists(string id)
        {
            return _context.LoadLine.Any(e => e.Id == id);
        }
    }
}
