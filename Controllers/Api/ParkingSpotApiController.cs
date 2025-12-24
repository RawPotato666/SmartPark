using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SmartPark.Data;
using SmartPark.Models;

namespace SmartPark.Controllers_Api
{
    [Route("api/v1/parkingspot")]
    [ApiController]
    public class ParkingSpotApiController : ControllerBase
    {
        private readonly SmartParkContext _context;

        public ParkingSpotApiController(SmartParkContext context)
        {
            _context = context;
        }

        // GET: api/ParkingSpotApi
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ParkingSpot>>> GetParkingSpots()
        {
            return await _context.ParkingSpots.ToListAsync();
        }

        // GET: api/ParkingSpotApi/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ParkingSpot>> GetParkingSpot(int id)
        {
            var parkingSpot = await _context.ParkingSpots.FindAsync(id);

            if (parkingSpot == null)
            {
                return NotFound();
            }

            return parkingSpot;
        }

        // PUT: api/ParkingSpotApi/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutParkingSpot(int id, ParkingSpot parkingSpot)
        {
            if (id != parkingSpot.Id)
            {
                return BadRequest();
            }

            _context.Entry(parkingSpot).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ParkingSpotExists(id))
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

        // POST: api/ParkingSpotApi
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<ParkingSpot>> PostParkingSpot(ParkingSpot parkingSpot)
        {
            _context.ParkingSpots.Add(parkingSpot);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetParkingSpot", new { id = parkingSpot.Id }, parkingSpot);
        }

        // DELETE: api/ParkingSpotApi/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteParkingSpot(int id)
        {
            var parkingSpot = await _context.ParkingSpots.FindAsync(id);
            if (parkingSpot == null)
            {
                return NotFound();
            }

            _context.ParkingSpots.Remove(parkingSpot);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ParkingSpotExists(int id)
        {
            return _context.ParkingSpots.Any(e => e.Id == id);
        }
    }
}
