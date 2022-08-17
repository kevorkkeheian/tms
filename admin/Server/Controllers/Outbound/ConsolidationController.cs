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
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Application.Shared.Models;

namespace Application.Server.Controllers.Outbound
{
    [Route("api/[controller]")]
    [ApiController]
    public class ConsolidationController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public ConsolidationController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: api/Consolidation
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Consolidation>>> GetConsolidation([FromQuery] string? storeId)
        {
            return await _context.Consolidation.Include(c => c.Store)
                                                    .Include(c => c.WorkDetail)
                                                    .Include(c => c.LicensePlate)
                                                    .Where(c => !String.IsNullOrEmpty(storeId) ? c.StoreId == storeId : true)
                                                    .ToListAsync();
        }

        [HttpGet("filter")]
        public async Task<ActionResult<IEnumerable<Consolidation>>> GetConsolidationFiltered([FromQuery] string routeHeaderId)
        {
            // Get stores that are included in the route document
            var routeLines = await _context.RouteLine.Where(r => r.RouteHeaderId == routeHeaderId).ToListAsync();
            string[] storeIds = routeLines.Select(r => r.StoreId).ToArray();

            // remove the consolidations that are included in the load lines
            var loadLines = await _context.LoadLine.Include(l => l.LoadHeader).Where(l => l.LoadHeader.RouteHeaderId == routeHeaderId).ToListAsync();

            // string[] consolidationIds = loadLines.Select(l => l.ConsolidationId).ToArray();
            string[] licensePlateIds = loadLines.Select(l => l.LicensePlateId).ToArray();

            return await _context.Consolidation.Include(c => c.Store).Include(c => c.LicensePlate).Where(c => storeIds.Contains(c.StoreId) && !licensePlateIds.Contains(c.Id)).ToListAsync();
        }
        

        // GET: api/Consolidation/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Consolidation>> GetConsolidationById(string id)
        {
            var consolidation = await _context.Consolidation.FindAsync(id);

            if (consolidation == null)
            {
                return NotFound();
            }

            return consolidation;
        }

        // PUT: api/Consolidation/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutConsolidation(string id, Consolidation consolidation)
        {
            if (id != consolidation.Id)
            {
                return BadRequest();
            }

            _context.Entry(consolidation).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ConsolidationExists(id))
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

        // POST: api/Consolidation
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Consolidation>> PostConsolidation(Consolidation consolidation)
        {
            var lp = await _context.LicensePlate.FirstOrDefaultAsync(l => l.Id == consolidation.LicensePlateId);
            var WorkDetail = await _context.WorkDetail.FirstOrDefaultAsync(w => w.Id == consolidation.WorkDetailId);

            if(lp is null || WorkDetail is null) {
                return NotFound();
            }
            else {
                lp.LicensePlateStatus = LicensePlateStatus.Consolidated;
                _context.Entry(lp).State = EntityState.Modified;

                WorkDetail.LicensePlateStatus = LicensePlateStatus.Consolidated; 
                _context.Entry(WorkDetail).State = EntityState.Modified;
            }
            _context.Consolidation.Add(consolidation);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (ConsolidationExists(consolidation.Id))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetConsolidation", new { id = consolidation.Id }, consolidation);
        }

        // DELETE: api/Consolidation/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteConsolidation(string id)
        {
            var consolidation = await _context.Consolidation.FindAsync(id);
            if (consolidation == null)
            {
                return NotFound();
            }

            _context.Consolidation.Remove(consolidation);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ConsolidationExists(string id)
        {
            return _context.Consolidation.Any(e => e.Id == id);
        }
    }
}
