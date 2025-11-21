using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using SmartPark.Models;
using SmartPark.Data;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace SmartPark.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly SmartParkContext _context;

    public HomeController(ILogger<HomeController> logger, SmartParkContext context)
    {
        _logger = logger;
        _context = context;
    }

    public IActionResult Index()
    {
        var lots = _context.ParkingLots.ToList();
        return View(lots);
    }

    public IActionResult ParkingSpots(int lotId, DateTime selectedDateTime)
    {
        var lot = _context.ParkingLots
                    .Include(l => l.ParkingSpots)
                    .FirstOrDefault(l => l.Id == lotId);

        if (lot == null)
            return NotFound();

        return View(lot);
    }

    // action to filter parking spots based on selected date and time, hopefully works
    public IActionResult ParkingSpotsFilter(int lotId, DateTime selectedDateTime)
    {
        var lot = _context.ParkingLots
                    .Include(l => l.ParkingSpots)
                    .ThenInclude(s => s.Reservations)
                    .FirstOrDefault(l => l.Id == lotId);

        if (lot == null)
            return NotFound();

        var occupiedStatus = new Dictionary<int, bool>();

        foreach (var spot in lot.ParkingSpots)
        {
            bool isOccupied = spot.Reservations.Any(r =>
                selectedDateTime >= r.Start &&
                selectedDateTime <= r.End
            );

            occupiedStatus[spot.Id] = isOccupied;
        }

    // Pass data to the view
    ViewBag.OccupiedStatus = occupiedStatus;
    ViewBag.SelectedDateTime = selectedDateTime.ToString("yyyy-MM-dd HH:mm");

    return View("ParkingSpots", lot);
}

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
