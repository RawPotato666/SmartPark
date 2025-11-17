using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SmartPark.Data;
using SmartPark.Models;

namespace SmartPark.Controllers
{
    public class ParkingSpotController : Controller
    {
        private readonly SmartParkContext _context;

        public ParkingSpotController(SmartParkContext context)
        {
            _context = context;
        }

        // GET: ParkingSpot
        public async Task<IActionResult> Index()
        {
            var smartParkContext = _context.ParkingSpots.Include(p => p.ParkingLot);
            return View(await smartParkContext.ToListAsync());
        }

        // GET: ParkingSpot/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var parkingSpot = await _context.ParkingSpots
                .Include(p => p.ParkingLot)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (parkingSpot == null)
            {
                return NotFound();
            }

            return View(parkingSpot);
        }

        // Check if a parking spot is occupied
        public async Task<bool> IsOccupied(int spotId)
        {
            var now = DateTime.Now;

            return await _context.Reservations
                .AnyAsync(r =>
                    r.ParkingSpot.Id == spotId &&
                    r.Start <= now &&
                    r.End >= now);
        }

        // GET: ParkingSpot/Create
        public IActionResult Create()
        {
            ViewData["ParkingLotId"] = new SelectList(_context.ParkingLots, "Id", "Id");
            return View();
        }

        // POST: ParkingSpot/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,IsDisabled,ParkingLotId")] ParkingSpot parkingSpot)
        {
            if (ModelState.IsValid)
            {
                _context.Add(parkingSpot);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ParkingLotId"] = new SelectList(_context.ParkingLots, "Id", "Id", parkingSpot.ParkingLotId);
            return View(parkingSpot);
        }

        // GET: ParkingSpot/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var parkingSpot = await _context.ParkingSpots.FindAsync(id);
            if (parkingSpot == null)
            {
                return NotFound();
            }
            ViewData["ParkingLotId"] = new SelectList(_context.ParkingLots, "Id", "Id", parkingSpot.ParkingLotId);
            return View(parkingSpot);
        }

        // POST: ParkingSpot/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,IsDisabled,ParkingLotId")] ParkingSpot parkingSpot)
        {
            if (id != parkingSpot.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(parkingSpot);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ParkingSpotExists(parkingSpot.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["ParkingLotId"] = new SelectList(_context.ParkingLots, "Id", "Id", parkingSpot.ParkingLotId);
            return View(parkingSpot);
        }

        // GET: ParkingSpot/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var parkingSpot = await _context.ParkingSpots
                .Include(p => p.ParkingLot)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (parkingSpot == null)
            {
                return NotFound();
            }

            return View(parkingSpot);
        }

        // POST: ParkingSpot/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var parkingSpot = await _context.ParkingSpots.FindAsync(id);
            if (parkingSpot != null)
            {
                _context.ParkingSpots.Remove(parkingSpot);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ParkingSpotExists(int id)
        {
            return _context.ParkingSpots.Any(e => e.Id == id);
        }
    }
}
