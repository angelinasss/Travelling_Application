using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Diagnostics;
using System.Linq;
using Travelling_Application.Models;
using Travelling_Application.ViewModels;

namespace Travelling_Application.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private ApplicationDbContext _context;
        private readonly IWebHostEnvironment _environment;

        public HomeController(ILogger<HomeController> logger, IWebHostEnvironment environment, ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
            _environment = environment;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult SearchFlights()
        {
            return View();
        }

        public IActionResult SearchCars()
        {
            return View();
        }

        public IActionResult SearchAttractions()
        {
            var cities = _context.Entertainment.Where(e => e.VerifiedByAdmin).Select(e => e.City).Distinct().ToList();
            var model = new SearchAttractionViewModel { Cities = cities, Results = new List<Entertainment>() };
            return View(model);
        }

        public IActionResult AttractionResults(string city, DateTime checkInDate, int adultsCount)
        {
            ViewBag.SelectedCity = city;
            ViewBag.CheckInDate = checkInDate;
            ViewBag.AdultsCount = adultsCount;

            var searchResults = _context.Entertainment
                .Where(e => e.City == city)
                .AsEnumerable() // Переключаемся на LINQ to Objects для использования индекса
                .Where(e => e.AvailableDates.Contains(checkInDate) && e.VerifiedByAdmin &&
                            e.AvailableDates.Zip(e.AmountOfTickets, (date, tickets) => new { date, tickets })
                                .Any(dt => dt.date == checkInDate && dt.tickets >= adultsCount))
                .ToList();

            var cities = _context.Entertainment.Where(e => e.VerifiedByAdmin).Select(e => e.City).Distinct().ToList();
            var model = new SearchAttractionViewModel { Cities = cities, Results = searchResults };
            return View("SearchAttractions", model);
        
        }

        //[Authorize(Roles = "admin")]
        //public IActionResult Privacy()
        //{
        //    return View();
        //}

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
