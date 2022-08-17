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

namespace Application.Server.Controllers.Outbound
{
    [Route("api/[controller]")]
    [ApiController]
    public class RouteLineController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public RouteLineController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/RouteLine
        [HttpGet]
        public async Task<ActionResult<IEnumerable<RouteLine>>> GetRouteLine()
        {
            return await _context.RouteLine.Include(r => r.Store).ThenInclude(s => s.Entity).OrderBy(r => r.LineNo).ToListAsync();
        }

        [HttpGet("filter")]
        public async Task<ActionResult<IEnumerable<RouteLine>>> GetRouteLineFiltered([FromQuery] string routeHeaderId)
        {
            return await _context.RouteLine.Include(r => r.Store).ThenInclude(s => s.Entity).Where(r => r.RouteHeaderId == routeHeaderId).OrderBy(r => r.LineNo).ToListAsync();
        }

        // GET: api/RouteLine/5
        [HttpGet("{id}")]
        public async Task<ActionResult<RouteLine>> GetRouteLine(string id)
        {
            var routeLine = await _context.RouteLine.Include(r => r.Store).ThenInclude(s => s.Entity).FirstOrDefaultAsync(r => r.Id == id);

            if (routeLine == null)
            {
                return NotFound();
            }

            return routeLine;
        }

        // PUT: api/RouteLine/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutRouteLine(string id, RouteLine routeLine)
        {
            if (id != routeLine.Id)
            {
                return BadRequest();
            }

            _context.Entry(routeLine).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RouteLineExists(id))
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

        // POST: api/RouteLine
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<RouteLine>> PostRouteLine(RouteLine routeLine)
        {
            var store = await _context.Store.Include(s => s.Entity).FirstOrDefaultAsync(s => s.Id == routeLine.StoreId);
            var entity = await _context.Entity.FirstOrDefaultAsync(e => e.Id == store.EntityId);

            _context.RouteLine.Add(routeLine);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetRouteLine", new { id = routeLine.Id, store = store, entity = entity }, routeLine);
        }

        // DELETE: api/RouteLine/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRouteLine(string id)
        {
            var routeLine = await _context.RouteLine.FindAsync(id);
            if (routeLine == null)
            {
                return NotFound();
            }

            _context.RouteLine.Remove(routeLine);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool RouteLineExists(string id)
        {
            return _context.RouteLine.Any(e => e.Id == id);
        }
    }
}
