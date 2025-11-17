using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using SmartPark.Models;
using SmartPark.Data;
using Microsoft.EntityFrameworkCore;

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
        var parkingSpots = _context.ParkingSpots.ToList();
        return View(parkingSpots);
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
