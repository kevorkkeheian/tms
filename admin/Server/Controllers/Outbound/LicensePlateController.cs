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
    public class LicensePlateController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public LicensePlateController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/LicensePlate
        [HttpGet]
        public async Task<ActionResult<IEnumerable<LicensePlate>>> GetLicensePlate([FromQuery] string? storeId, [FromQuery] string? lpStatus)
        {
            
            return await _context.LicensePlate.Include(l => l.Store)
                                                .Where(l => !String.IsNullOrEmpty(storeId) ? l.StoreId == storeId : true)
                                                .Where(l => !String.IsNullOrEmpty(lpStatus) ? l.LicensePlateStatus == Enum.Parse<LicensePlateStatus>(lpStatus) : true)
                                                .OrderBy(l => l.Code).ToListAsync();
        }

        // [HttpGet("filter")]
        // public async Task<ActionResult<LicensePlate>> GetFilteredLicensePlate([FromQuery] string code)
        // {
        //     return await _context.LicensePlate.Include(l => l.Store).FirstOrDefaultAsync(l =>l.Code == code);
        // }

        [HttpGet("filter")]
        public async Task<ActionResult<IEnumerable<LicensePlate>>> GetConsolidationFiltered([FromQuery] string? routeHeaderId)
        {
            // Get stores that are included in the route document
            var routeLines = await _context.RouteLine.Where(r => r.RouteHeaderId == routeHeaderId).ToListAsync();
            string[] storeIds = routeLines.Select(r => r.StoreId).ToArray();

            // remove the consolidations that are included in the load lines
            var loadLines = await _context.LoadLine.Include(l => l.LoadHeader).Where(l => l.LoadHeader.RouteHeaderId == routeHeaderId).ToListAsync();

            // string[] consolidationIds = loadLines.Select(l => l.ConsolidationId).ToArray();
            string[] licensePlateIds = loadLines.Select(l => l.LicensePlateId).ToArray();

            return await _context.LicensePlate.Include(c => c.Store)
                                                .Where(c => storeIds.Contains(c.StoreId) && !licensePlateIds.Contains(c.Id))
                                                .ToListAsync();
        }

        // GET: api/LicensePlate/5
        [HttpGet("{id}")]
        public async Task<ActionResult<LicensePlate>> GetLicensePlateById(string id)
        {
            var licensePlate = await _context.LicensePlate.FindAsync(id);

            if (licensePlate == null)
            {
                return NotFound();
            }

            return licensePlate;
        }

        // PUT: api/LicensePlate/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutLicensePlate(string id, LicensePlate licensePlate)
        {
            if (id != licensePlate.Id)
            {
                return BadRequest();
            }

            _context.Entry(licensePlate).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!LicensePlateExists(id))
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

        // POST: api/LicensePlate
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<LicensePlate>> PostLicensePlate(LicensePlate licensePlate)
        {
            _context.LicensePlate.Add(licensePlate);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetLicensePlateById", new { id = licensePlate.Id }, licensePlate);
        }


        [HttpPost("generate")]
        public async Task<ActionResult> GenerateLicensePlate(LicensePlateGenerateModel lpGenerate)
        {
            string lastNo = "";
            var last = await _context.LicensePlate.Where(l => l.StoreId == lpGenerate.StoreId).OrderBy(l => l.Code).LastOrDefaultAsync();

            if(last is not null) {
                lastNo = last?.Code;
            }

            
            var doucmentGenerator = new DoucmentGenerater();
            var store = await _context.Store.FirstOrDefaultAsync(s => s.Id == lpGenerate.StoreId);

            for(int i = 0; i < lpGenerate.Quantity; i++) {
                
                var licensePlate = new LicensePlate()
                {
                    StoreId = lpGenerate.StoreId,
                    Code = await doucmentGenerator.CreateDocumentNoAsync(lastNo, store.Code),
                    LicensePlateStatus = LicensePlateStatus.Pending
                };

                _context.LicensePlate.Add(licensePlate);
                await _context.SaveChangesAsync();

                var request = await GetLicensePlateById(licensePlate.Id);
                last = request.Value;

                if(last is not null) {
                    lastNo = last?.Code;
                }

            }   
            

            return Ok();
        }

        // DELETE: api/LicensePlate/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteLicensePlate(string id)
        {
            var licensePlate = await _context.LicensePlate.FindAsync(id);
            if (licensePlate == null)
            {
                return NotFound();
            }

            _context.LicensePlate.Remove(licensePlate);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool LicensePlateExists(string id)
        {
            return _context.LicensePlate.Any(e => e.Id == id);
        }
    }
}
