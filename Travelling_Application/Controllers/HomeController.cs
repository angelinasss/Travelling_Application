using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using Travelling_Application.Models;
using Travelling_Application.ViewModels;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Travelling_Application.Controllers
{
    public class AirTicketComparer : IEqualityComparer<AirTicket>
    {
        public bool Equals(AirTicket x, AirTicket y)
        {
            // Два объекта считаются одинаковыми, если у них одинаковый Id
            return x.Id == y.Id;
        }

        public int GetHashCode(AirTicket obj)
        {
            // Используем Id для генерации хеш-кода
            return obj.Id.GetHashCode();
        }
    }
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

            foreach (var accomodation in validAccomodations)
            {
                // Проверить, есть ли у отеля комнаты, которые доступны на все даты
                var availableRooms = _context.Rooms
                    .Where(r => r.AccomodationId == accomodation.Id)
                    .ToList();

                var roomsAvail = new List<string>();

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
                        roomsAvail.Add(room.RoomName);
                    }
                }

                accomodation.AvailableRoomsNames = roomsAvail;
                _context.Accomodation.Update(accomodation);
                _context.SaveChanges();
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

     
        public async Task<IActionResult> AttractiveOffers()
        {
            var currentUser = _context.Users.FirstOrDefault(u => u.UserName == User.Identity.Name);

            var favoriteCarIds = _context.FavoriteCars
                                       .Where(fc => fc.UserId == currentUser.Id)
                                       .Select(fc => fc.CarId)
                                       .ToList();

            var favoriteCarCities = _context.Car
                                      .Where(c => favoriteCarIds.Contains(c.Id))
                                      .Select(c => c.City)
                                      .ToList();

            var favoriteAttractionIds = _context.FavoriteAttractions
                                       .Where(fc => fc.UserId == currentUser.Id)
                                       .Select(fc => fc.AttractionId)
                                       .ToList();

            var favoriteAttractionCities = _context.Entertainment
                                      .Where(c => favoriteAttractionIds.Contains(c.Id))
                                      .Select(c => c.City)
                                      .ToList();

            var favoriteAirTicketIds = _context.FavoriteAirTickets
                                       .Where(fc => fc.UserId == currentUser.Id)
                                       .Select(fc => fc.AirTicketId)
                                       .ToList();

            var favoriteAirTicketCities = _context.AirTicket
                                        .Where(c => favoriteAirTicketIds.Contains(c.Id))
                                        .Select(c => new { c.CityFrom, c.CityTo })
                                        .ToList();

            var favoriteAccomodationIds = _context.FavoriteAccomodations
                                      .Where(fc => fc.UserId == currentUser.Id)
                                      .Select(fc => fc.AccomodationId)
                                      .ToList();

            var favoriteAccomodationCities = _context.Accomodation
                                      .Where(c => favoriteAccomodationIds.Contains(c.Id))
                                      .Select(c => c.City)
                                      .ToList();

            var allFavoriteCities = new HashSet<string>();

            // Добавление городов в HashSet
            allFavoriteCities.UnionWith(favoriteCarCities);
            allFavoriteCities.UnionWith(favoriteAttractionCities);
            foreach (var city in favoriteAirTicketCities)
            {
                allFavoriteCities.Add(city.CityFrom);
                allFavoriteCities.Add(city.CityTo);
            }
            allFavoriteCities.UnionWith(favoriteAccomodationCities);

            var cars = _context.Car
             .Where(car => allFavoriteCities.Contains(car.City) && car.VerifiedByAdmin) // Фильтруем машины, чтобы выбрать только те, которые находятся в любимых городах
             .GroupBy(car => car.City) // Группируем машины по городу
             .Select(group => group.OrderBy(car => car.Cost).FirstOrDefault()) // Из каждой группы выбираем машину с минимальной стоимостью
             .ToList();

            var attractions = _context.Entertainment
             .Where(attraction => allFavoriteCities.Contains(attraction.City) && attraction.VerifiedByAdmin) // Фильтруем машины, чтобы выбрать только те, которые находятся в любимых городах
             .GroupBy(attraction => attraction.City) // Группируем машины по городу
             .Select(group => group.OrderBy(attraction => attraction.Cost).FirstOrDefault()) // Из каждой группы выбираем машину с минимальной стоимостью
             .ToList();

            var accomodations = _context.Accomodation
             .Where(accomodation => allFavoriteCities.Contains(accomodation.City) && accomodation.VerifiedByAdmin) // Фильтруем машины, чтобы выбрать только те, которые находятся в любимых городах
             .GroupBy(accomodation => accomodation.City) // Группируем машины по городу
             .Select(group => group.OrderBy(accomodation => accomodation.MinCost).FirstOrDefault()) // Из каждой группы выбираем машину с минимальной стоимостью
             .ToList();

            var airticketsFrom = _context.AirTicket
                .Where(airticket => allFavoriteCities.Contains(airticket.CityFrom) && airticket.VerifiedByAdminECTicket)
                .GroupBy(airticket => airticket.CityFrom)
                .Select(group => group.OrderBy(airticket => airticket.CostEC).FirstOrDefault())
                .ToList();

            var airticketsTo = _context.AirTicket
                .Where(airticket => allFavoriteCities.Contains(airticket.CityTo) && airticket.VerifiedByAdminECTicket)
                .GroupBy(airticket => airticket.CityTo)
                .Select(group => group.OrderBy(airticket => airticket.CostEC).FirstOrDefault())
                .ToList();

            var uniqueAirtickets = airticketsFrom.Union(airticketsTo, new AirTicketComparer()).ToList();

            DateTime today = DateTime.Now;
            var airTickets = new List<AirTicket>();

            foreach(var airticket in uniqueAirtickets)
            {
                if(airticket.DepartureTime < today)
                {
                   
                }
                else
                {
                    airTickets.Add(airticket);
                }
            }

            var model = new VerifiedObjectsViewModel()
            {
                VerifiedCars = cars,
                VerifiedAirTickets = airTickets,
                VerifiedAttractions = attractions,
                VerifiedAccomodation = accomodations
            };

            return View(model);
        }

        [HttpPost]
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
                RejectedBooking = false,
                RejectedMessage = string.Empty,
                AmountOfTickets = amountOfTickets,
                Date = date, 
                CanceledBooking = false
            };
            for(int i = 0; i < attraction.AmountOfTickets.Count; i++)
            {
                if (attraction.AvailableDates[i] == date)
                {
                    var tickets = attraction.AmountOfTickets[i];
                    attraction.AmountOfTickets[i] = tickets - amountOfTickets;
                }
            }

            _context.Entertainment.Update(attraction);

            _context.BookingAttractions.Add(model);
            await _context.SaveChangesAsync();

            return RedirectToAction("AttractionResults", new { city = attraction.City, checkInDate = date, adultsCount = amountOfTickets });
        }

        [HttpPost]
        public async Task<IActionResult> BookAttractionFav(string attractionId, int amountOfTickets, DateTime date)
        {
            var currentUser = _context.Users.FirstOrDefault(u => u.UserName == User.Identity.Name);
            int AttractionId = int.Parse(attractionId);
            var attraction = _context.Entertainment.Find(AttractionId);

            var model = new BookingAttraction
            {
                UserId = currentUser.Id,
                AttractionId = AttractionId,
                VerifiedBooking = false,
                RejectedBooking = false,
                RejectedMessage = string.Empty,
                AmountOfTickets = amountOfTickets,
                Date = date,
                CanceledBooking = false
            };
            for (int i = 0; i < attraction.AmountOfTickets.Count; i++)
            {
                if (attraction.AvailableDates[i] == date)
                {
                    var tickets = attraction.AmountOfTickets[i];
                    attraction.AmountOfTickets[i] = tickets - amountOfTickets;
                }
            }

            _context.Entertainment.Update(attraction);

            _context.BookingAttractions.Add(model);
            await _context.SaveChangesAsync();

            return RedirectToAction("ShowFavoriteItems");
        }

        [HttpPost]
        public async Task<IActionResult> BookCarFav(int carId, DateTime dateIn, DateTime dateOut)
        {
            var currentUser = _context.Users.FirstOrDefault(u => u.UserName == User.Identity.Name);
            var car = _context.Car.Find(carId);

            var model = new BookingCar
            {
                UserId = currentUser.Id,
                CarId = carId,
                VerifiedBooking = false,
                RejectedBooking = false,
                RejectedMessage = string.Empty,
                DateOfDeparture = dateIn,
                ReturnDate = dateOut,
                CanceledBooking = false
            };

            _context.BookingCars.Add(model);

            int index = -1;

            for (int i = 0; i < car.StartDates.Count; i++)
            {
                if (car.StartDates[i] <= dateIn && car.EndDates[i] >= dateOut)
                {
                    index = i;
                    break;
                }
            }

            if (index == -1)
            {
                throw new InvalidOperationException("No suitable date range found.");
            }

            DateTime oldStartDate = car.StartDates[index];
            DateTime oldEndDate = car.EndDates[index];

            // Обновляем массивы
            car.StartDates[index] = oldStartDate;
            car.EndDates[index] = dateIn;

            var newStartDates = car.StartDates.ToList();
            var newEndDates = car.EndDates.ToList();

            newStartDates.Add(dateOut);
            newEndDates.Add(oldEndDate);

            car.StartDates = newStartDates;
            car.EndDates = newEndDates;

            _context.Car.Update(car);
            await _context.SaveChangesAsync();

            return RedirectToAction("ShowFavoriteItems");
        }

        [HttpPost]
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
                RejectedBooking = false,
                RejectedMessage = string.Empty,
                DateOfDeparture = dateIn,
                ReturnDate = dateOut,
                CanceledBooking = false
            };

            _context.BookingCars.Add(model);

            int index = -1;

            for (int i = 0; i < car.StartDates.Count; i++)
            {
                if (car.StartDates[i] <= dateIn && car.EndDates[i] >= dateOut)
                {
                    index = i;
                    break;
                }
            }

            if (index == -1)
            {
                throw new InvalidOperationException("No suitable date range found.");
            }

            DateTime oldStartDate = car.StartDates[index];
            DateTime oldEndDate = car.EndDates[index];

            // Обновляем массивы
            car.StartDates[index] = oldStartDate;
            car.EndDates[index] = dateIn;

            var newStartDates = car.StartDates.ToList();
            var newEndDates = car.EndDates.ToList();

            newStartDates.Add(dateOut);
            newEndDates.Add(oldEndDate);

            car.StartDates = newStartDates;
            car.EndDates = newEndDates;

            _context.Car.Update(car);
            await _context.SaveChangesAsync();

            return RedirectToAction("CarResults", new { city = car.City, checkInDate = dateIn, checkOutDate = dateOut });
        }

        [HttpPost]
        public async Task<IActionResult> BookAttractionOffer(int attractionId, int adultsCount, DateTime checkInDate)
        {
            var currentUser = _context.Users.FirstOrDefault(u => u.UserName == User.Identity.Name);
            var attraction = _context.Entertainment.Find(attractionId);

            bool exist = false;
            DateTime date = checkInDate;
            int ticketsCount = adultsCount;

            for (int i = 0; i < attraction.AmountOfTickets.Count; i++)
            {
                if (attraction.AvailableDates[i] == date)
                {
                    if (attraction.AmountOfTickets[i] >= ticketsCount)
                    {
                        exist = true;
                        break;
                    }
                }
            }

            if (exist)
            {

                var model = new BookingAttraction
                {
                    UserId = currentUser.Id,
                    AttractionId = attractionId,
                    VerifiedBooking = false,
                    RejectedBooking = false,
                    RejectedMessage = string.Empty,
                    AmountOfTickets = adultsCount,
                    Date = checkInDate,
                    CanceledBooking = false
                };

                for (int i = 0; i < attraction.AmountOfTickets.Count; i++)
                {
                    if (attraction.AvailableDates[i] == checkInDate)
                    {
                        var tickets = attraction.AmountOfTickets[i];
                        attraction.AmountOfTickets[i] = tickets - adultsCount;
                    }
                }

                _context.Entertainment.Update(attraction);

                _context.BookingAttractions.Add(model);
                await _context.SaveChangesAsync();

                return Json(new { success = true });
            }
            else
            {
                return Json(new { success = false, message = "The selected amount of tickets are unavailable for booking." });
            }
        }

        [HttpPost]
        public async Task<IActionResult> BookAccomodationOffer(int accomodationId, DateTime dateIn, DateTime dateOut, int rooms, int adults, string selectedRoom)
        {
            var currentUser = _context.Users.FirstOrDefault(u => u.UserName == User.Identity.Name);
            var accomodation = _context.Accomodation.Find(accomodationId);
            var room = _context.Rooms.FirstOrDefault(r => r.AccomodationId == accomodationId && r.RoomName == selectedRoom);

            var availableDates = room.AvailableDatesRoom;
            int j = 0;
            // Проверить, доступны ли все даты в интервале [checkInDate, checkOutDate)
            bool allDatesAvailable = true;
            for (var date = dateIn; date < dateOut; date = date.AddDays(1))
            {
                if (!availableDates.Contains(date) || room.AmountOfAvailableSameRooms[j] < rooms)
                {
                    allDatesAvailable = false;
                    break;
                }
                j++;
            }

            if (allDatesAvailable)
            {

                var model = new BookingAccomodation
                {
                    UserId = currentUser.Id,
                    AccomodationId = accomodationId,
                    VerifiedBooking = false,
                    RejectedBooking = false,
                    RejectedMessage = string.Empty,
                    DateOfDeparture = dateIn,
                    ReturnDate = dateOut,
                    Adults = rooms,
                    RoomId = room.ID,
                    TypeOfRoom = room.RoomName,
                    CanceledBooking = false
                };

                _context.BookingAccomodations.Add(model);
                await _context.SaveChangesAsync();

                int i = 0;
                foreach (var date in room.AvailableDatesRoom)
                {
                    if (date >= dateIn && date < dateOut)
                    {
                        room.AmountOfAvailableSameRooms[i] = room.AmountOfAvailableSameRooms[i] - rooms;
                    }
                    i++;
                }

                _context.Rooms.Update(room);
                await _context.SaveChangesAsync();

                return Json(new { success = true });
            }
            else
            {
                return Json(new { success = false, message = "The selected dates for current type of room are unavailable for booking." });
            }
        }

        [HttpPost]
        public async Task<IActionResult> BookFlightECOffer(int airticketId, DateTime dateIn, DateTime dateOut, string passengers)
        {
            var currentUser = _context.Users.FirstOrDefault(u => u.UserName == User.Identity.Name);
            var airticket = _context.AirTicket.Find(airticketId);

            var pass = Int32.Parse(passengers);

            bool exist = false;

            if(airticket.AmountOfTicketsEC >= pass)
            {
                exist = true;
            }

            if (exist)
            {

                var model = new BookingAirTicket
                {
                    UserId = currentUser.Id,
                    AirTicketId = airticketId,
                    VerifiedBooking = false,
                    RejectedBooking = false,
                    RejectedMessage = string.Empty,
                    DateOfDeparture = dateIn,
                    ReturnDate = dateOut,
                    Passengers = pass,
                    TypeClass = "EC",
                    CanceledBooking = false
                };

                var amountTick = airticket.AmountOfTicketsEC - pass;
                airticket.AmountOfTicketsEC = amountTick;
                _context.AirTicket.Update(airticket);
                _context.BookingAirTickets.Add(model);
                await _context.SaveChangesAsync();

                return Json(new { success = true });
            }
            else
            {
                return Json(new { success = false, message = "The selected count of passengers are unavailable for booking." });
            }
        }

        [HttpPost]
        public async Task<IActionResult> BookCarOffer(int carId, DateTime checkInDate, DateTime checkOutDate)
        {
            var car = _context.Car.Find(carId);
            bool exist = false;
            DateTime dateIn = checkInDate;
            DateTime dateOut = checkOutDate;

            for (int i = 0; i < car.StartDates.Count; i++)
            {
                if (checkInDate >= car.StartDates[i] && checkInDate < car.EndDates[i] && checkOutDate <= car.EndDates[i] && checkOutDate > car.StartDates[i])
                {
                    exist = true;
                    break;
                }
            }

            if (exist)
            {
                var currentUser = _context.Users.FirstOrDefault(u => u.UserName == User.Identity.Name);

                var model = new BookingCar
                {
                    UserId = currentUser.Id,
                    CarId = carId,
                    VerifiedBooking = false,
                    RejectedBooking = false,
                    RejectedMessage = string.Empty,
                    DateOfDeparture = dateIn,
                    ReturnDate = dateOut,
                    CanceledBooking = false
                };

                _context.BookingCars.Add(model);

                int index = -1;

                for (int i = 0; i < car.StartDates.Count; i++)
                {
                    if (car.StartDates[i] <= dateIn && car.EndDates[i] >= dateOut)
                    {
                        index = i;
                        break;
                    }
                }

                if (index == -1)
                {
                    throw new InvalidOperationException("No suitable date range found.");
                }

                DateTime oldStartDate = car.StartDates[index];
                DateTime oldEndDate = car.EndDates[index];

                // Обновляем массивы
                car.StartDates[index] = oldStartDate;
                car.EndDates[index] = dateIn;

                var newStartDates = car.StartDates.ToList();
                var newEndDates = car.EndDates.ToList();

                newStartDates.Add(dateOut);
                newEndDates.Add(oldEndDate);

                car.StartDates = newStartDates;
                car.EndDates = newEndDates;

                _context.Car.Update(car);
                await _context.SaveChangesAsync();

                return Json(new { success = true });
            }
            else
            {
                return Json(new { success = false, message = "The selected date range is unavailable for booking." });
            }
        }
        
        [HttpPost]
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
                RejectedBooking = false,
                RejectedMessage = string.Empty,
                DateOfDeparture = dateIn,
                ReturnDate = dateOut,
                Passengers = passengers,
                TypeClass = "EC",
                CanceledBooking = false
            };

            var amountTick = airticket.AmountOfTicketsEC - passengers;
            airticket.AmountOfTicketsEC = amountTick;
            _context.AirTicket.Update(airticket);
            _context.BookingAirTickets.Add(model);
            await _context.SaveChangesAsync();
     
            return RedirectToAction("FlightResults", new { cityFrom = airticket.CityFrom, cityTo = airticket.CityTo, checkInDate = dateIn, checkOutDate = dateOut, tripType = tripTypee, adultsCount = passengers });
        }

        [HttpPost]
        public async Task<IActionResult> BookFlightECFav(string airticketId, DateTime dateIn, DateTime dateOut, int passengers)
        {
            var currentUser = _context.Users.FirstOrDefault(u => u.UserName == User.Identity.Name);
            int AirticketId = int.Parse(airticketId);
            var airticket = _context.AirTicket.Find(AirticketId);

            var model = new BookingAirTicket
            {
                UserId = currentUser.Id,
                AirTicketId = AirticketId,
                VerifiedBooking = false,
                RejectedBooking = false,
                RejectedMessage = string.Empty,
                DateOfDeparture = dateIn,
                ReturnDate = dateOut,
                Passengers = passengers,
                TypeClass = "EC",
                CanceledBooking = false
            };

            var amountTick = airticket.AmountOfTicketsEC - passengers;
            airticket.AmountOfTicketsEC = amountTick;
            _context.AirTicket.Update(airticket);
            _context.BookingAirTickets.Add(model);
            await _context.SaveChangesAsync();

            return RedirectToAction("ShowFavoriteItems");
        }

        [HttpPost]
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
                RejectedBooking = false,
                RejectedMessage = string.Empty,
                DateOfDeparture = dateIn,
                ReturnDate = dateOut,
                Passengers = passengers,
                TypeClass = "BC",
                CanceledBooking = false
            };

            var amountTick = airticket.AmountOfTicketsBC;
            airticket.AmountOfTicketsBC = amountTick - passengers;
            _context.AirTicket.Update(airticket);
            _context.BookingAirTickets.Add(model);
            await _context.SaveChangesAsync();

            return RedirectToAction("FlightResults", new { cityFrom = airticket.CityFrom, cityTo = airticket.CityTo, checkInDate = dateIn, checkOutDate = dateOut, tripType = tripTypee, adultsCount = passengers });
        }

        [HttpPost]
        public async Task<IActionResult> BookFlightBCFav(string airticketId, DateTime dateIn, DateTime dateOut, int passengers)
        {
            var currentUser = _context.Users.FirstOrDefault(u => u.UserName == User.Identity.Name);
            int AirticketId = int.Parse(airticketId);
            var airticket = _context.AirTicket.Find(AirticketId);

            var model = new BookingAirTicket
            {
                UserId = currentUser.Id,
                AirTicketId = AirticketId,
                VerifiedBooking = false,
                RejectedBooking = false,
                RejectedMessage = string.Empty,
                DateOfDeparture = dateIn,
                ReturnDate = dateOut,
                Passengers = passengers,
                TypeClass = "BC",
                CanceledBooking = false
            };

            var amountTick = airticket.AmountOfTicketsBC;
            airticket.AmountOfTicketsBC = amountTick - passengers;
            _context.AirTicket.Update(airticket);
            _context.BookingAirTickets.Add(model);
            await _context.SaveChangesAsync();

            return RedirectToAction("ShowFavoriteItems");
        }

        [HttpPost]
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
                RejectedBooking = false,
                RejectedMessage = string.Empty,
                DateOfDeparture = dateIn,
                ReturnDate = dateOut,
                Passengers = passengers,
                TypeClass = "FC",
                CanceledBooking = false
            };

            var amountTick = airticket.AmountOfTicketsFC;
            airticket.AmountOfTicketsFC = amountTick - passengers;
            _context.AirTicket.Update(airticket);
            _context.BookingAirTickets.Add(model);
            await _context.SaveChangesAsync();

            return RedirectToAction("FlightResults", new { cityFrom = airticket.CityFrom, cityTo = airticket.CityTo, checkInDate = dateIn, checkOutDate = dateOut, tripType = tripTypee, adultsCount = passengers });
        }

        [HttpPost]
        public async Task<IActionResult> BookFlightFCFav(string airticketId, DateTime dateIn, DateTime dateOut, int passengers)
        {
            var currentUser = _context.Users.FirstOrDefault(u => u.UserName == User.Identity.Name);
            int AirticketId = int.Parse(airticketId);
            var airticket = _context.AirTicket.Find(AirticketId);

            var model = new BookingAirTicket
            {
                UserId = currentUser.Id,
                AirTicketId = AirticketId,
                VerifiedBooking = false,
                RejectedBooking = false,
                RejectedMessage = string.Empty,
                DateOfDeparture = dateIn,
                ReturnDate = dateOut,
                Passengers = passengers,
                TypeClass = "FC",
                CanceledBooking = false
            };

            var amountTick = airticket.AmountOfTicketsFC;
            airticket.AmountOfTicketsFC = amountTick - passengers;
            _context.AirTicket.Update(airticket);
            _context.BookingAirTickets.Add(model);
            await _context.SaveChangesAsync();

            return RedirectToAction("ShowFavoriteItems");
        }

        [HttpPost]
        public async Task<IActionResult> BookAccomodation(string accomodationId, DateTime dateIn, DateTime dateOut, int rooms, int adults, string selectedRoom)
        {
            var currentUser = _context.Users.FirstOrDefault(u => u.UserName == User.Identity.Name);
            int AccomodationId = int.Parse(accomodationId);
            var accomodation = _context.Accomodation.Find(AccomodationId);
            var room = _context.Rooms.FirstOrDefault(r => r.AccomodationId == AccomodationId && r.RoomName == selectedRoom);

            var model = new BookingAccomodation
            {
                UserId = currentUser.Id,
                AccomodationId = AccomodationId,
                VerifiedBooking = false,
                RejectedBooking = false,
                RejectedMessage = string.Empty,
                DateOfDeparture = dateIn,
                ReturnDate = dateOut,
                Adults = rooms,
                RoomId = room.ID,
                TypeOfRoom = room.RoomName,
                CanceledBooking = false
            };

            _context.BookingAccomodations.Add(model);
            await _context.SaveChangesAsync();

            int i = 0;
            foreach(var date in room.AvailableDatesRoom)
            {
                if(date >= dateIn && date < dateOut)
                {
                    room.AmountOfAvailableSameRooms[i] = room.AmountOfAvailableSameRooms[i] - rooms;
                }
                i++;
            }

            _context.Rooms.Update(room);
            await _context.SaveChangesAsync();

            return RedirectToAction("AccomodationResults", new { city = accomodation.City, checkInDate = dateIn, checkOutDate = dateOut, adults, rooms});
        }

        [HttpPost]
        public async Task<IActionResult> BookAccomodationFav(string accomodationId, DateTime dateIn, DateTime dateOut, int rooms, int adults, string selectedRoom)
        {
            var currentUser = _context.Users.FirstOrDefault(u => u.UserName == User.Identity.Name);
            int AccomodationId = int.Parse(accomodationId);
            var accomodation = _context.Accomodation.Find(AccomodationId);
            var room = _context.Rooms.FirstOrDefault(r => r.AccomodationId == AccomodationId && r.RoomName == selectedRoom);

            var model = new BookingAccomodation
            {
                UserId = currentUser.Id,
                AccomodationId = AccomodationId,
                VerifiedBooking = false,
                RejectedBooking = false,
                RejectedMessage = string.Empty,
                DateOfDeparture = dateIn,
                ReturnDate = dateOut,
                Adults = rooms,
                RoomId = room.ID,
                TypeOfRoom = room.RoomName,
                CanceledBooking = false
            };

            _context.BookingAccomodations.Add(model);
            await _context.SaveChangesAsync();

            int i = 0;
            foreach (var date in room.AvailableDatesRoom)
            {
                if (date >= dateIn && date < dateOut)
                {
                    room.AmountOfAvailableSameRooms[i] = room.AmountOfAvailableSameRooms[i] - rooms;
                }
                i++;
            }

            _context.Rooms.Update(room);
            await _context.SaveChangesAsync();

            return RedirectToAction("ShowFavoriteItems");
        }

        [HttpPost]
        public async Task<IActionResult> AddToFavoriteAttraction(int attractionId, int amountOfTickets, DateTime date)
        {
            var currentUser = _context.Users.FirstOrDefault(u => u.UserName == User.Identity.Name);

            var model = new FavoriteAttraction
            {
                UserId = currentUser.Id,
                AttractionId = attractionId,
                AmountOfTickets = amountOfTickets,
                Date = date
            };

            _context.FavoriteAttractions.Add(model);
            await _context.SaveChangesAsync();

            return Json(new { isFavorite = true });
        }

        [HttpPost]
        public async Task<IActionResult> AddToFavoriteCar(int carId, DateTime dateIn, DateTime dateOut)
        {
            var currentUser = _context.Users.FirstOrDefault(u => u.UserName == User.Identity.Name);

            var model = new FavoriteCar
            {
                UserId = currentUser.Id,
                CarId = carId,
                DateOfDeparture = dateIn,
                ReturnDate = dateOut
            };

            _context.FavoriteCars.Add(model);
            await _context.SaveChangesAsync();

            return Json(new { isFavorite = true });
        }

        [HttpPost]
        public async Task<IActionResult> AddToFavoriteAirTicketEC(int airticketId, DateTime dateIn, DateTime dateOut, int passengers)
        {
            var currentUser = _context.Users.FirstOrDefault(u => u.UserName == User.Identity.Name);

            var model = new FavoriteAirTicket
            {
                UserId = currentUser.Id,
                AirTicketId = airticketId,
                DateOfDeparture = dateIn,
                ReturnDate = dateOut,
                Passengers = passengers,
                TypeClass = "EC"
            };

            _context.FavoriteAirTickets.Add(model);
            await _context.SaveChangesAsync();

            return Json(new { isFavorite = true });
        }

        [HttpPost]
        public async Task<IActionResult> AddToFavoriteAirTicketBC(int airticketId, DateTime dateIn, DateTime dateOut, int passengers)
        {
            var currentUser = _context.Users.FirstOrDefault(u => u.UserName == User.Identity.Name);

            var model = new FavoriteAirTicket
            {
                UserId = currentUser.Id,
                AirTicketId = airticketId,
                DateOfDeparture = dateIn,
                ReturnDate = dateOut,
                Passengers = passengers,
                TypeClass = "BC"
            };

            _context.FavoriteAirTickets.Add(model);
            await _context.SaveChangesAsync();

            return Json(new { isFavorite = true });
        }

        [HttpPost]
        public async Task<IActionResult> AddToFavoriteAirTicketFC(int airticketId, DateTime dateIn, DateTime dateOut, int passengers)
        {
            var currentUser = _context.Users.FirstOrDefault(u => u.UserName == User.Identity.Name);

            var model = new FavoriteAirTicket
            {
                UserId = currentUser.Id,
                AirTicketId = airticketId,
                DateOfDeparture = dateIn,
                ReturnDate = dateOut,
                Passengers = passengers,
                TypeClass = "FC"
            };

            _context.FavoriteAirTickets.Add(model);
            await _context.SaveChangesAsync();

            return Json(new { isFavorite = true });
        }

        [HttpPost]
        public async Task<IActionResult> AddToFavoriteAccomodation(int accomodationId, string dateIn, string dateOut, int rooms, string selectedRoom)
        {
            var currentUser = _context.Users.FirstOrDefault(u => u.UserName == User.Identity.Name);
            var room = _context.Rooms.FirstOrDefault(r => r.AccomodationId == accomodationId && r.RoomName == selectedRoom);

            DateTime date1 = DateTime.Parse(dateIn);

            DateTime date2 = DateTime.Parse(dateOut);

            var model = new FavoriteAccomodation
            {
                UserId = currentUser.Id,
                AccomodationId = accomodationId,
                DateOfDeparture = date1,
                ReturnDate = date2,
                Rooms = rooms,
                TypeOfRoom = selectedRoom,
                RoomId = room.ID
            };

            _context.FavoriteAccomodations.Add(model);
            await _context.SaveChangesAsync();

            return Json(new { isFavorite = true });
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
