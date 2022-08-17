#nullable disable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Application.Server.Data;
using Application.Shared.Models.Warehouse;
using Application.Shared.Models.Enum;

namespace Application.Server.Controllers.Warehouse
{
    [Route("api/[controller]")]
    [ApiController]
    public class WorkDetailController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public WorkDetailController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/WorkDetail
        [HttpGet]
        public async Task<ActionResult<IEnumerable<WorkDetail>>> GetWorkDetail([FromQuery] string? storeId, [FromQuery] string? lpStatus)
        {
            // convert lpStatus to enum
            var lpStatusEnum = !String.IsNullOrEmpty(lpStatus) ? Enum.Parse<LicensePlateStatus>(lpStatus) : LicensePlateStatus.Pending;

            //if(!String.IsNullOrEmpty(storeId))
            //{
                return await _context.WorkDetail.Include(w => w.Store)
                                                .Where(w => !String.IsNullOrEmpty(storeId) ? w.StoreId == storeId : true)
                                                .Where(w => !String.IsNullOrEmpty(lpStatus) ? w.LicensePlateStatus == lpStatusEnum : true)
                                                .ToListAsync();
            //}
            // else
            // {
            //     return await _context.WorkDetail.ToListAsync();
            // }
        }


        [HttpGet("filter")]
        public async Task<ActionResult<WorkDetail>> GetFilteredWorkDetail([FromQuery] string lp)
        {
            return await _context.WorkDetail.FirstOrDefaultAsync(w => w.LicensePlate == lp);
        }

        // GET: api/WorkDetail/5
        [HttpGet("{id}")]
        public async Task<ActionResult<WorkDetail>> GetWorkDetail(string id)
        {
            var workDetail = await _context.WorkDetail.FindAsync(id);

            if (workDetail == null)
            {
                return NotFound();
            }

            return workDetail;
        }

        // PUT: api/WorkDetail/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutWorkDetail(string id, WorkDetail workDetail)
        {
            if (id != workDetail.Id)
            {
                return BadRequest();
            }

            _context.Entry(workDetail).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!WorkDetailExists(id))
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

        // POST: api/WorkDetail
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<WorkDetail>> PostWorkDetail(WorkDetail workDetail)
        {
            _context.WorkDetail.Add(workDetail);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetWorkDetail", new { id = workDetail.Id }, workDetail);
        }

        // DELETE: api/WorkDetail/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteWorkDetail(string id)
        {
            var workDetail = await _context.WorkDetail.FindAsync(id);
            if (workDetail == null)
            {
                return NotFound();
            }

            _context.WorkDetail.Remove(workDetail);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool WorkDetailExists(string id)
        {
            return _context.WorkDetail.Any(e => e.Id == id);
        }
    }
}
