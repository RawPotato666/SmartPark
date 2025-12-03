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

        ViewBag.SelectedDateTime = DateTime.Now.ToString("yyyy-MM-ddTHH:mm");
        ViewBag.SelectedEndTime = DateTime.Now.AddHours(1).ToString("yyyy-MM-ddTHH:mm");

        return View(lot);
    }

    public IActionResult ParkingSpotsFilter(int lotId, DateTime selectedDateTime, DateTime selectedEndTime)
    {
        var lot = _context.ParkingLots
        .Include(p => p.ParkingSpots)
        .FirstOrDefault(x => x.Id == lotId);

        foreach (var spot in lot.ParkingSpots)
        {
            spot.IsOccupied = _context.Reservations
            .Any(r => r.ParkingSpotId == spot.Id &&
                selectedDateTime <= r.End &&
                selectedEndTime >= r.Start
                );                                            
        }

        ViewBag.SelectedDateTime = selectedDateTime.ToString("yyyy-MM-ddTHH:mm");
        ViewBag.SelectedEndTime = selectedEndTime.ToString("yyyy-MM-ddTHH:mm");

    return View("ParkingSpots", lot);
    }

    public IActionResult Privacy()
    {
        return View();
    }

    public IActionResult About()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
