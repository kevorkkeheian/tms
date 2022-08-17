// #nullable disable
// using System;
// using System.Collections.Generic;
// using System.Linq;
// using System.Threading.Tasks;
// using Microsoft.AspNetCore.Http;
// using Microsoft.AspNetCore.Mvc;
// using Microsoft.EntityFrameworkCore;
// using Application.Server.Data;
// using Application.Shared.Models.Outbound;
// using Application.Server.Services;
// using Application.Shared.Models.Enum;
// using Application.Shared.Models.Warehouse;

// namespace Application.Server.Controllers.Outbound
// {
//     [Route("api/[controller]")]
//     [ApiController]
//     public class WorkDetailController : ControllerBase
//     {
//         private readonly ApplicationDbContext _context;

//         public WorkDetailController(ApplicationDbContext context)
//         {
//             _context = context;
//         }

//         // GET: api/WorkDetail
//         [HttpGet]
//         public async Task<ActionResult<IEnumerable<WorkDetail>>> GetWorkDetail([FromQuery] string? storeId, [FromQuery] string? lpStatus)
//         {
            
//             return await _context.WorkDetail.Include(l => l.Store)
//                                                 .Where(l => !String.IsNullOrEmpty(storeId) ? l.StoreId == storeId : true)
//                                                 .Where(l => !String.IsNullOrEmpty(lpStatus) ? l.LicensePlateStatus == Enum.Parse<LicensePlateStatus>(lpStatus) : true)
//                                                 .OrderBy(l => l.Store.Code).ToListAsync();
//         }

//         [HttpGet("filter")]
//         public async Task<ActionResult<WorkDetail>> GetFilteredWorkDetail([FromQuery] string code)
//         {
//             return await _context.WorkDetail.Include(l => l.Store).FirstOrDefaultAsync(l =>l.Store.Code == code);
//         }

//         // GET: api/WorkDetail/5
//         [HttpGet("{id}")]
//         public async Task<ActionResult<WorkDetail>> GetWorkDetailById(string id)
//         {
//             var workDetail = await _context.WorkDetail.FindAsync(id);

//             if (workDetail == null)
//             {
//                 return NotFound();
//             }

//             return workDetail;
//         }

//         // PUT: api/WorkDetail/5
//         // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
//         // [HttpPut("{id}")]
//         // public async Task<IActionResult> PutWorkDetail(string id, WorkDetail workDetail)
//         // {
//         //     if (id != workDetail.Id)
//         //     {
//         //         return BadRequest();
//         //     }

//         //     _context.Entry(workDetail).State = EntityState.Modified;

//         //     try
//         //     {
//         //         await _context.SaveChangesAsync();
//         //     }
//         //     catch (DbUpdateConcurrencyException)
//         //     {
//         //         if (!WorkDetailExists(id))
//         //         {
//         //             return NotFound();
//         //         }
//         //         else
//         //         {
//         //             throw;
//         //         }
//         //     }

//         //     return NoContent();
//         // }

//         // POST: api/WorkDetail
//         // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
//         // [HttpPost]
//         // public async Task<ActionResult<WorkDetail>> PostWorkDetail(WorkDetail workDetail)
//         // {
//         //     _context.WorkDetail.Add(workDetail);
//         //     await _context.SaveChangesAsync();

//         //     return CreatedAtAction("GetWorkDetailById", new { id = workDetail.Id }, workDetail);
//         // }


//         // [HttpPost("generate")]
//         // public async Task<ActionResult> GenerateWorkDetail(WorkDetailGenerateModel lpGenerate)
//         // {
//         //     string lastNo = "";
//         //     var last = await _context.WorkDetail.Where(l => l.StoreId == lpGenerate.StoreId).OrderBy(l => l.Code).LastOrDefaultAsync();

//         //     if(last is not null) {
//         //         lastNo = last?.Code;
//         //     }

            
//         //     var doucmentGenerator = new DoucmentGenerater();
//         //     var store = await _context.Store.FirstOrDefaultAsync(s => s.Id == lpGenerate.StoreId);

//         //     for(int i = 0; i < lpGenerate.Quantity; i++) {
                
//         //         var workDetail = new WorkDetail()
//         //         {
//         //             StoreId = lpGenerate.StoreId,
//         //             Code = await doucmentGenerator.CreateDocumentNoAsync(lastNo, store.Code),
//         //             WorkDetailStatus = WorkDetailStatus.Pending
//         //         };

//         //         _context.WorkDetail.Add(workDetail);
//         //         await _context.SaveChangesAsync();

//         //         var request = await GetWorkDetailById(workDetail.Id);
//         //         last = request.Value;

//         //         if(last is not null) {
//         //             lastNo = last?.Code;
//         //         }

//         //     }   
            

//         //     return Ok();
//         // }

//         // DELETE: api/WorkDetail/5
//         // [HttpDelete("{id}")]
//         // public async Task<IActionResult> DeleteWorkDetail(string id)
//         // {
//         //     var workDetail = await _context.WorkDetail.FindAsync(id);
//         //     if (workDetail == null)
//         //     {
//         //         return NotFound();
//         //     }

//         //     _context.WorkDetail.Remove(workDetail);
//         //     await _context.SaveChangesAsync();

//         //     return NoContent();
//         // }

//         private bool WorkDetailExists(string id)
//         {
//             return _context.WorkDetail.Any(e => e.Id == id);
//         }
//     }
// }
