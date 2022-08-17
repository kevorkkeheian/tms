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
using Application.Server.Services;
using Application.Shared.Models.Enum;

namespace Application.Server.Controllers.Outbound
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoadHeaderController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public LoadHeaderController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/LoadHeader
        [HttpGet]
        public async Task<ActionResult<IEnumerable<LoadHeader>>> GetLoadHeader()
        {
            return await _context.LoadHeader.Include(l => l.RouteHeader).ToListAsync();
        }

        // GET: api/LoadHeader/5
        [HttpGet("{id}")]
        public async Task<ActionResult<LoadHeader>> GetLoadHeader(string id)
        {
            var loadHeader = await _context.LoadHeader.FindAsync(id);

            if (loadHeader == null)
            {
                return NotFound();
            }

            return loadHeader;
        }

        [HttpGet("filter")]
        public async Task<ActionResult<LoadHeader>> GetLoadHeaderByFilter([FromQuery] string truckId)
        {
            var loadHeader = await _context.LoadHeader.Include(l => l.RouteHeader).ThenInclude(r => r.Truck)
                                                        .Include(l => l.RouteHeader).ThenInclude(r => r.Driver)
                                                        .FirstOrDefaultAsync(l => l.RouteHeader.TruckId == truckId
                                                            && (l.LoadStatus == LoadStatus.Pending || l.LoadStatus == LoadStatus.Loading));


            // var loadHeader = await _context.LoadHeader.Include(l => l.RouteHeader)
            //                         .FirstOrDefaultAsync(l => l.RouteHeader.TruckId == truckId);

            if (loadHeader == null)
            {
                return NotFound();
            }

            return loadHeader;
        }

        // PUT: api/LoadHeader/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutLoadHeader(string id, LoadHeader loadHeader)
        {
            if (id != loadHeader.Id)
            {
                return BadRequest();
            }

            _context.Entry(loadHeader).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!LoadHeaderExists(id))
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

        // POST: api/LoadHeader
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<LoadHeader>> PostLoadHeader(LoadHeader loadHeader)
        {
            string lastDocumentNo = "";
            var lastDocument = await _context.LoadHeader.OrderByDescending(x => x.DocumentNo).FirstOrDefaultAsync();

            if(lastDocument is not null) {
                lastDocumentNo = lastDocument?.DocumentNo;
            }

            var doucmentGenerator = new DoucmentGenerater();
            loadHeader.DocumentNo = await doucmentGenerator.CreateDocumentNoAsync(lastDocumentNo, "LD");

            _context.LoadHeader.Add(loadHeader);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetLoadHeader", new { id = loadHeader.Id }, loadHeader);
        }

        // DELETE: api/LoadHeader/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteLoadHeader(string id)
        {
            var loadHeader = await _context.LoadHeader.FindAsync(id);
            if (loadHeader == null)
            {
                return NotFound();
            }

            _context.LoadHeader.Remove(loadHeader);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool LoadHeaderExists(string id)
        {
            return _context.LoadHeader.Any(e => e.Id == id);
        }
    }
}
