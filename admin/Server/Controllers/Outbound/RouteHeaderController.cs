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
    public class RouteHeaderController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public RouteHeaderController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/RouteHeader
        [HttpGet]
        public async Task<ActionResult<IEnumerable<RouteHeader>>> GetRouteHeader()
        {
            return await _context.RouteHeader.Include(r => r.Truck).Include(r => r.Driver).OrderByDescending(r => r.DocumentNo).ToListAsync();
        }

        [HttpGet("filter")]
        public async Task<ActionResult<IEnumerable<RouteHeader>>> GetFilteredRouteHeader([FromQuery] string? routeStatus, [FromQuery] string? truckId)
        {
            var routeStatusEnum = Enum.Parse<RouteStatus>(routeStatus);
            var routes =  await _context.RouteHeader.Include(r => r.Truck).Include(r => r.Driver)
                                                .Where(r => r.RouteStatus == routeStatusEnum)
                                                // if truck id is not null truck id contains truck id
                                                .Where(r => !String.IsNullOrEmpty(truckId) ? r.TruckId.Contains(truckId) : true)
                                                .ToListAsync();
            return routes;
        }

        // GET: api/RouteHeader/5
        [HttpGet("{id}")]
        public async Task<ActionResult<RouteHeader>> GetRouteHeader(string id)
        {
            var routeHeader = await _context.RouteHeader.FindAsync(id);

            if (routeHeader == null)
            {
                return NotFound();
            }

            return routeHeader;
        }

        // PUT: api/RouteHeader/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutRouteHeader(string id, RouteHeader routeHeader)
        {
            if (id != routeHeader.Id)
            {
                return BadRequest();
            }

            _context.Entry(routeHeader).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RouteHeaderExists(id))
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

        // POST: api/RouteHeader
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<RouteHeader>> PostRouteHeader(RouteHeader routeHeader)
        {
            routeHeader.DocumentNo = await CreateDocumentNoAsync();

            _context.RouteHeader.Add(routeHeader);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetRouteHeader", new { id = routeHeader.Id }, routeHeader);
        }

        // DELETE: api/RouteHeader/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRouteHeader(string id)
        {
            var routeHeader = await _context.RouteHeader.FindAsync(id);
            if (routeHeader == null)
            {
                return NotFound();
            }

            // get all routelines of this route header
            var routeLines = await _context.RouteLine.Where(r => r.RouteHeaderId == id).ToListAsync();

            // delete all routelines of this route header
            _context.RouteLine.RemoveRange(routeLines);

            _context.RouteHeader.Remove(routeHeader);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private async Task<string> CreateDocumentNoAsync()
        {
            var lastRouteHeader = await _context.RouteHeader.OrderByDescending(x => x.DocumentNo).FirstOrDefaultAsync();
            string lastDocumentNo = lastRouteHeader?.DocumentNo;

            

            if(lastRouteHeader is null) {
                return $"TRS-{DateTime.Today.ToString("yy")}-000001";;
            }
            else {
                var lastDocumentNoSplit = lastDocumentNo.Split('-')[2];
                var newDocumentNoNumber = int.Parse(lastDocumentNoSplit) + 1;
                
                if(newDocumentNoNumber.ToString().Length == 1)
                {
                    return $"TRS-{DateTime.Today.ToString("yy")}-00000{newDocumentNoNumber}";
                }
                else if(newDocumentNoNumber.ToString().Length == 2)
                {
                    return $"TRS-{DateTime.Today.ToString("yy")}-0000{newDocumentNoNumber}";
                }
                else if(newDocumentNoNumber.ToString().Length == 3)
                {
                    return $"TRS-{DateTime.Today.ToString("yy")}-000{newDocumentNoNumber}";
                }
                else if(newDocumentNoNumber.ToString().Length == 4)
                {
                    return $"TRS-{DateTime.Today.ToString("yy")}-00{newDocumentNoNumber}";
                }
                else if(newDocumentNoNumber.ToString().Length == 5)
                {
                    return $"TRS-{DateTime.Today.ToString("yy")}-0{newDocumentNoNumber}";
                }
                else
                {
                    return $"TRS-{DateTime.Today.ToString("yy")}-{newDocumentNoNumber}";
                }
            }
            
        }
        

        private bool RouteHeaderExists(string id)
        {
            return _context.RouteHeader.Any(e => e.Id == id);
        }
    }
}
