#nullable disable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Application.Server.Data;
using Application.Shared.Models.Org;

namespace Application.Server.Controllers.Org
{
    [Route("api/[controller]")]
    [ApiController]
    public class TruckController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public TruckController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Truck
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Truck>>> GetTruck()
        {
            return await _context.Truck.ToListAsync();
        }

        // GET: api/Truck/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Truck>> GetTruck(string id)
        {
            var truck = await _context.Truck.FindAsync(id);

            if (truck == null)
            {
                return NotFound();
            }

            return truck;
        }

        // PUT: api/Truck/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTruck(string id, Truck truck)
        {
            if (id != truck.Id)
            {
                return BadRequest();
            }

            _context.Entry(truck).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TruckExists(id))
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

        // POST: api/Truck
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Truck>> PostTruck(Truck truck)
        {
            _context.Truck.Add(truck);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetTruck", new { id = truck.Id }, truck);
        }

        // DELETE: api/Truck/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTruck(string id)
        {
            var truck = await _context.Truck.FindAsync(id);
            if (truck == null)
            {
                return NotFound();
            }

            _context.Truck.Remove(truck);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool TruckExists(string id)
        {
            return _context.Truck.Any(e => e.Id == id);
        }
    }
}
