using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SmartPark.Data;
using SmartPark.Filters;
using SmartPark.Models;


namespace SmartPark.Controllers_Api
{
    [Route("api/v1/parkinglot")]
    [ApiController]
    public class ParkingLotApiController : ControllerBase
    {
        private readonly SmartParkContext _context;

        public ParkingLotApiController(SmartParkContext context)
        {
            _context = context;
        }

        // GET: api/ParkingLotApi
        [HttpGet]
        [ApiKeyAuth]
        public async Task<ActionResult<IEnumerable<ParkingLot>>> GetParkingLots()
        {
            return await _context.ParkingLots.ToListAsync();
        }

        // GET: api/ParkingLotApi/5
        [HttpGet("{id}")]
        [ApiKeyAuth]
        public async Task<ActionResult<ParkingLot>> GetParkingLot(int id)
        {
            var parkingLot = await _context.ParkingLots.FindAsync(id);

            if (parkingLot == null)
            {
                return NotFound();
            }

            return parkingLot;
        }

        // PUT: api/ParkingLotApi/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutParkingLot(int id, ParkingLot parkingLot)
        {
            if (id != parkingLot.Id)
            {
                return BadRequest();
            }

            _context.Entry(parkingLot).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ParkingLotExists(id))
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

        // POST: api/ParkingLotApi
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<ParkingLot>> PostParkingLot(ParkingLot parkingLot)
        {
            _context.ParkingLots.Add(parkingLot);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetParkingLot", new { id = parkingLot.Id }, parkingLot);
        }

        // DELETE: api/ParkingLotApi/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteParkingLot(int id)
        {
            var parkingLot = await _context.ParkingLots.FindAsync(id);
            if (parkingLot == null)
            {
                return NotFound();
            }

            _context.ParkingLots.Remove(parkingLot);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ParkingLotExists(int id)
        {
            return _context.ParkingLots.Any(e => e.Id == id);
        }
    }
}
