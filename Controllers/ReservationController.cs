using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SmartPark.Data;
using SmartPark.Models;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace SmartPark.Controllers
{
    public class ReservationController : Controller
    {
        private readonly SmartParkContext _context;

        public ReservationController(SmartParkContext context)
        {
            _context = context;
        }

        // GET: Reservation
        public async Task<IActionResult> Index()
        {
            var smartParkContext = _context.Reservations
                .Include(r => r.ParkingSpot)
                .ThenInclude(ps => ps.ParkingLot)
                .Include(r => r.User);
            return View(await smartParkContext.ToListAsync());
        }

        [Authorize]
        public IActionResult Reserve(int Id, DateTime? start, DateTime? end)
        {
            var spot = _context.ParkingSpots.FirstOrDefault(x => x.Id == Id);
            if (spot == null) return NotFound();

            var reservation = new Reservation
            {
                ParkingSpotId = Id,
                Start = start ?? DateTime.Now,
                End = end ?? DateTime.Now.AddHours(1),
                ParkingSpot = spot
            };

            ViewBag.ParkingSpotDisplayId = spot.DisplayId;

            return View(reservation);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public IActionResult Reserve([Bind("UserId,ParkingSpotId,Start,End")] Reservation reservation)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            void PopulateSpotDisplay()
            {
                var spot = _context.ParkingSpots.FirstOrDefault(x => x.Id == reservation.ParkingSpotId);
                if (spot != null)
                {
                    reservation.ParkingSpot = spot;
                    ViewBag.ParkingSpotDisplayId = spot.DisplayId;
                }
            }
            
            if (string.IsNullOrEmpty(userId))
                return RedirectToAction("Login", "Account");

            reservation.UserId = userId;

            if (reservation.Start >= reservation.End)
            {
                ModelState.AddModelError("", "End time must be after start time.");
                PopulateSpotDisplay();
                return View(reservation);
            }

            if (reservation.Start < DateTime.Now + TimeSpan.FromMinutes(1))
            {
                ModelState.AddModelError("", "Start time cannot be in the past.");
                PopulateSpotDisplay();
                return View(reservation);
            }

            // Check for overlapping reservations for same parking spot
            bool conflict = _context.Reservations.Any(r =>
                r.ParkingSpotId == reservation.ParkingSpotId &&
                reservation.Start < r.End &&
                reservation.End > r.Start
            );

            if (conflict)
            {
                ModelState.AddModelError("", "This parking spot is already reserved for that time.");
                PopulateSpotDisplay();
                return View(reservation);
            }


            _context.Reservations.Add(reservation);
            _context.SaveChanges();

            return RedirectToAction("MyReservations", reservation);
        }

        [Authorize]
        public IActionResult MyReservations()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var reservations = _context.Reservations
                            .Include(r => r.ParkingSpot)
                                .ThenInclude(s => s.ParkingLot)
                            .Where(r => r.UserId == userId)
                            .OrderByDescending(r => r.Start)
                            .ToList();

            return View(reservations);
        }

        public IActionResult Cancel(int id)
        {
            var reservation = _context.Reservations
                .FirstOrDefault(r => r.Id == id);

            if (reservation == null)
            {
                return NotFound();
            }

            return View(reservation);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ConfirmCancel(int id)
        {
            var reservation = _context.Reservations.FirstOrDefault(r => r.Id == id);

            if (reservation == null)
            {
                return NotFound();
            }

            _context.Reservations.Remove(reservation);
            _context.SaveChanges();

            return RedirectToAction("MyReservations");
        }

        // GET: Reservation/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var reservation = await _context.Reservations
                .Include(r => r.ParkingSpot)
                .ThenInclude(ps => ps.ParkingLot)
                .Include(r => r.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (reservation == null)
            {
                return NotFound();
            }

            return View(reservation);
        }

        // GET: Reservation/Create
        public IActionResult Create()
        {
            ViewData["ParkingSpotId"] = new SelectList(_context.ParkingSpots, "Id", "Id");
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id");
            return View();
        }

        // POST: Reservation/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,UserId,ParkingSpotId,Start,End")] Reservation reservation)
        {
            if (ModelState.IsValid)
            {
                _context.Add(reservation);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ParkingSpotId"] = new SelectList(_context.ParkingSpots, "Id", "Id", reservation.ParkingSpotId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", reservation.UserId);
            return View(reservation);
        }

        // GET: Reservation/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var reservation = await _context.Reservations.FindAsync(id);
            if (reservation == null)
            {
                return NotFound();
            }
            ViewData["ParkingSpotId"] = new SelectList(_context.ParkingSpots, "Id", "Id", reservation.ParkingSpotId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", reservation.UserId);
            return View(reservation);
        }

        // POST: Reservation/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,UserId,ParkingSpotId,Start,End")] Reservation reservation)
        {
            if (id != reservation.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(reservation);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ReservationExists(reservation.Id))
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
            ViewData["ParkingSpotId"] = new SelectList(_context.ParkingSpots, "Id", "Id", reservation.ParkingSpotId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", reservation.UserId);
            return View(reservation);
        }

        // GET: Reservation/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var reservation = await _context.Reservations
                .Include(r => r.ParkingSpot)
                .Include(r => r.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (reservation == null)
            {
                return NotFound();
            }

            return View(reservation);
        }

        // POST: Reservation/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var reservation = await _context.Reservations.FindAsync(id);
            if (reservation != null)
            {
                _context.Reservations.Remove(reservation);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ReservationExists(int id)
        {
            return _context.Reservations.Any(e => e.Id == id);
        }
    }
}
