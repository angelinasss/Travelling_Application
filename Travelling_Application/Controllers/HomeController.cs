using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Runtime.ConstrainedExecution;
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
            var cities = _context.Accomodation.Where(e => e.VerifiedByAdmin).Select(e => e.City).Distinct().ToList();
            var model = new SearchAccomodationViewModel { Cities = cities, Results = new List<Accomodation>() };
            return View(model);
        }

        public IActionResult SearchFlights()
        {
            var citiesFrom = _context.AirTicket.Where(e => e.VerifiedByAdminECTicket || e.VerifiedByAdminBCTicket || e.VerifiedByAdminFCTicket).Select(e => e.CityFrom).Distinct().ToList();
            var citiesTo = _context.AirTicket.Where(e => e.VerifiedByAdminECTicket || e.VerifiedByAdminBCTicket || e.VerifiedByAdminFCTicket).Select(e => e.CityTo).Distinct().ToList();
            var model = new SearchFlightViewModel { CitiesFrom = citiesFrom, CitiesTo = citiesTo, Results = new List<AirTicket>() };
            return View(model);
        }

        public IActionResult SearchCars()
        {
            var cities = _context.Car.Where(e => e.VerifiedByAdmin).Select(e => e.City).Distinct().ToList();
            var model = new SearchCarViewModel { Cities = cities, Results = new List<Car>() };
            return View(model);
        }

        public IActionResult SearchAttractions()
        {
            var cities = _context.Entertainment.Where(e => e.VerifiedByAdmin).Select(e => e.City).Distinct().ToList();
            var model = new SearchAttractionViewModel { Cities = cities, Results = new List<Entertainment>() };
            return View(model);
        }

        public IActionResult AccomodationResults(string city, DateTime checkInDate, DateTime checkOutDate, int adults, int rooms)
        {
            ViewBag.SelectedCity = city;
            ViewBag.CheckInDate = checkInDate;
            ViewBag.CheckOutDate = checkOutDate;
            ViewBag.Adults = adults;
            ViewBag.Rooms = rooms;

            // Получить все отели в указанном городе
            var accomodations = _context.Accomodation
                .Where(e => e.City == city)
                .ToList(); // Преобразовать в список для дальнейшей обработки

            var validAccomodations = new List<Accomodation>();

            foreach (var accomodation in accomodations)
            {
                // Проверить, есть ли у отеля комнаты, которые доступны на все даты
                var availableRooms = _context.Rooms
                    .Where(r => r.AccomodationId == accomodation.Id)
                    .ToList();

                foreach (var room in availableRooms)
                {
                    // Получить доступные даты для этой комнаты
                    var availableDates = room.AvailableDatesRoom;
                    int i = 0;
                    // Проверить, доступны ли все даты в интервале [checkInDate, checkOutDate)
                    bool allDatesAvailable = true;
                    for (var date = checkInDate; date < checkOutDate; date = date.AddDays(1))
                    {
                        if (!availableDates.Contains(date) || room.AmountOfAvailableSameRooms[i] < rooms)
                        {
                            allDatesAvailable = false;
                            break;
                        }
                        i++;
                    }

                    if (allDatesAvailable)
                    {
                        validAccomodations.Add(accomodation);
                        break; // Не нужно проверять другие комнаты этого отеля
                    }
                }
            }

            var searchResults = validAccomodations;

            var cities = _context.Accomodation.Where(e => e.VerifiedByAdmin).Select(e => e.City).Distinct().ToList();
            var model = new SearchAccomodationViewModel { Cities = cities, Results = searchResults };
            return View("Index", model);

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

        public IActionResult CarResults(string city, DateTime checkInDate, DateTime checkOutDate)
        {
            ViewBag.SelectedCity = city;
            ViewBag.CheckInDate = checkInDate;
            ViewBag.CheckOutDate = checkOutDate;

            var searchResults = _context.Car
                .Where(e => e.City == city)
                .AsEnumerable() // Switch to LINQ to Objects to use methods like Any
                .Where(e => e.VerifiedByAdmin &&
                            e.StartDates.Zip(e.EndDates, (start, end) => new { Start = start, End = end })
                                        .Any(interval => interval.Start < checkInDate && checkOutDate < interval.End))
                .ToList();

            var cities = _context.Car.Where(e => e.VerifiedByAdmin).Select(e => e.City).Distinct().ToList();
            var model = new SearchCarViewModel { Cities = cities, Results = searchResults };
            return View("SearchCars", model);

        }

        public IActionResult FlightResults(string cityFrom, string cityTo, DateTime checkInDate, DateTime checkOutDate, string tripType, int adultsCount)
        {
            bool isOneWayTrip = tripType == "oneWay";

            ViewBag.SelectedCityFrom = cityFrom;
            ViewBag.SelectedCityTo = cityTo;
            ViewBag.CheckInDate = checkInDate;
            ViewBag.CheckOutDate = checkOutDate;
            ViewBag.AdultsCount = adultsCount;
            ViewBag.OneWayTrip = isOneWayTrip;
            var searchResults = new List<AirTicket>();
            var backTickets = new List<AirTicket>();

            if (isOneWayTrip)
            {
                searchResults = _context.AirTicket
               .Where(e => e.CityTo == cityTo && e.CityFrom == cityFrom && e.DepartureTime.Date == checkInDate && (e.AmountOfTicketsEC >= adultsCount || e.AmountOfTicketsBC >= adultsCount || e.AmountOfTicketsFC >= adultsCount))
               .AsEnumerable() // Switch to LINQ to Objects to use methods like Any
               .Where(e => e.VerifiedByAdminECTicket || e.VerifiedByAdminBCTicket || e.VerifiedByAdminFCTicket)
               .ToList();
            }
            else
            {
                searchResults = _context.AirTicket
              .Where(e => e.CityTo == cityTo && e.CityFrom == cityFrom && e.DepartureTime.Date == checkInDate && (e.AmountOfTicketsEC >= adultsCount || e.AmountOfTicketsBC >= adultsCount || e.AmountOfTicketsFC >= adultsCount))
              .AsEnumerable() // Switch to LINQ to Objects to use methods like Any
              .Where(e => e.VerifiedByAdminECTicket || e.VerifiedByAdminBCTicket || e.VerifiedByAdminFCTicket)
              .ToList();

                backTickets = _context.AirTicket
              .Where(e => e.CityTo == cityFrom && e.CityFrom == cityTo && e.DepartureTime.Date == checkOutDate && (e.AmountOfTicketsEC >= adultsCount || e.AmountOfTicketsBC >= adultsCount || e.AmountOfTicketsFC >= adultsCount))
              .AsEnumerable() // Switch to LINQ to Objects to use methods like Any
              .Where(e => e.VerifiedByAdminECTicket || e.VerifiedByAdminBCTicket || e.VerifiedByAdminFCTicket)
              .ToList();

                foreach(var ticket in backTickets)
                {
                    searchResults.Add(ticket);
                }
            
            }

           

            var citiesFrom = _context.AirTicket.Where(e => e.VerifiedByAdminECTicket || e.VerifiedByAdminBCTicket || e.VerifiedByAdminFCTicket).Select(e => e.CityFrom).Distinct().ToList();
            var citiesTo = _context.AirTicket.Where(e => e.VerifiedByAdminECTicket || e.VerifiedByAdminBCTicket || e.VerifiedByAdminFCTicket).Select(e => e.CityTo).Distinct().ToList();
            var model = new SearchFlightViewModel { CitiesFrom = citiesFrom, CitiesTo = citiesTo, Results = searchResults };
            return View("SearchFlights", model);

        }

        public async Task<IActionResult> BookAttraction(string attractionId, int amountOfTickets, DateTime date)
        {
            var currentUser = _context.Users.FirstOrDefault(u => u.UserName == User.Identity.Name);
            int AttractionId = int.Parse(attractionId);
            var attraction = _context.Entertainment.Find(AttractionId);

            var model = new BookingAttraction
            {
                UserId = currentUser.Id,
                AttractionId = AttractionId,
                VerifiedBooking = false,
                AmountOfTickets = amountOfTickets,
                Date = date
            };

            _context.BookingAttractions.Add(model);
            await _context.SaveChangesAsync();

            return RedirectToAction("AttractionResults", new { city = attraction.City, checkInDate = date, adultsCount = amountOfTickets });
        }

        public async Task<IActionResult> BookCar(string carId, DateTime dateIn, DateTime dateOut)
        {
            var currentUser = _context.Users.FirstOrDefault(u => u.UserName == User.Identity.Name);
            int CarId = int.Parse(carId);
            var car = _context.Car.Find(CarId);

            var model = new BookingCar
            {
                UserId = currentUser.Id,
                CarId = CarId,
                VerifiedBooking = false,
                DateOfDeparture = dateIn,
                ReturnDate = dateOut
            };

            _context.BookingCars.Add(model);
            await _context.SaveChangesAsync();

            return RedirectToAction("CarResults", new { city = car.City, checkInDate = dateIn, checkOutDate = dateOut });
        }

        public async Task<IActionResult> BookFlightEC(string airticketId, DateTime dateIn, DateTime dateOut, int passengers, bool tripTypee)
        {
            var currentUser = _context.Users.FirstOrDefault(u => u.UserName == User.Identity.Name);
            int AirticketId = int.Parse(airticketId);
            var airticket = _context.AirTicket.Find(AirticketId);

            var model = new BookingAirTicket
            {
                UserId = currentUser.Id,
                AirTicketId = AirticketId,
                VerifiedBooking = false,
                DateOfDeparture = dateIn,
                ReturnDate = dateOut,
                Passengers = passengers
            };

            _context.BookingAirTickets.Add(model);
            await _context.SaveChangesAsync();
     
            return RedirectToAction("FlightResults", new { cityFrom = airticket.CityFrom, cityTo = airticket.CityTo, checkInDate = dateIn, checkOutDate = dateOut, tripType = tripTypee, adultsCount = passengers });
        }

        public async Task<IActionResult> BookFlightBC(string airticketId, DateTime dateIn, DateTime dateOut, int passengers, bool tripTypee)
        {
            var currentUser = _context.Users.FirstOrDefault(u => u.UserName == User.Identity.Name);
            int AirticketId = int.Parse(airticketId);
            var airticket = _context.AirTicket.Find(AirticketId);

            var model = new BookingAirTicket
            {
                UserId = currentUser.Id,
                AirTicketId = AirticketId,
                VerifiedBooking = false,
                DateOfDeparture = dateIn,
                ReturnDate = dateOut,
                Passengers = passengers
            };

            _context.BookingAirTickets.Add(model);
            await _context.SaveChangesAsync();

            return RedirectToAction("FlightResults", new { cityFrom = airticket.CityFrom, cityTo = airticket.CityTo, checkInDate = dateIn, checkOutDate = dateOut, tripType = tripTypee, adultsCount = passengers });
        }

        public async Task<IActionResult> BookFlightFC(string airticketId, DateTime dateIn, DateTime dateOut, int passengers, bool tripTypee)
        {
            var currentUser = _context.Users.FirstOrDefault(u => u.UserName == User.Identity.Name);
            int AirticketId = int.Parse(airticketId);
            var airticket = _context.AirTicket.Find(AirticketId);

            var model = new BookingAirTicket
            {
                UserId = currentUser.Id,
                AirTicketId = AirticketId,
                VerifiedBooking = false,
                DateOfDeparture = dateIn,
                ReturnDate = dateOut,
                Passengers = passengers
            };

            _context.BookingAirTickets.Add(model);
            await _context.SaveChangesAsync();

            return RedirectToAction("FlightResults", new { cityFrom = airticket.CityFrom, cityTo = airticket.CityTo, checkInDate = dateIn, checkOutDate = dateOut, tripType = tripTypee, adultsCount = passengers });
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
