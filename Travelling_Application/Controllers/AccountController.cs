using Auth0.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Runtime.ConstrainedExecution;
using System.Security.Claims;
using Travelling_Application.Models;
using Travelling_Application.ViewModels;

namespace Travelling_Application.Controllers
{
    public class AccountController : Controller
    {
        private ApplicationDbContext _context;
        private readonly IWebHostEnvironment _environment;

        public AccountController(IWebHostEnvironment environment, ApplicationDbContext context)
        {
            _context = context;
            _environment = environment;
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterModel model)
        {
            if (ModelState.IsValid)
            {
                User user = await _context.Users.FirstOrDefaultAsync(u => u.UserName == model.UserName);
                if (user == null)
                {
                    string filePath = "C:\\Users\\Angelina\\Pictures\\Camera Roll\\Avatar-Profile-No-Background.png";
                    byte[] fileBytes = System.IO.File.ReadAllBytes(filePath);
                    // Обновляем данные пользователя
                    // добавляем пользователя в бд
                    user = new User
                    {
                        UserName = model.UserName,
                        Password = model.Password,
                        Name = "",
                        Birthday = new DateTime(),
                        Nationality = "",
                        Email = "",
                        PhoneNumber = "",
                        Sex = "",
                        Role = model.AccountType,
                        CountryCode = "",
                        UserPhotoUrl = fileBytes
                    };

                    _context.Users.Add(user);
                    await _context.SaveChangesAsync();

                    await Authenticate(user); // аутентификация

                    return RedirectToAction("Index", "Home");
                }
                else
                    ModelState.AddModelError("", "Username already exists");
            }
            return View(model);
        }
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginModel model)
        {
            if (ModelState.IsValid)
            {
                User user = await _context.Users
                    .FirstOrDefaultAsync(u => u.UserName == model.UserName && u.Password == model.Password);
                if (user != null)
                {
                    await Authenticate(user); // аутентификация

                    return RedirectToAction("Index", "Home");
                }
                ModelState.AddModelError("", "Username or password is incorrect");
            }
            return View(model);
        }
        private async Task Authenticate(User user)
        {
            // создаем один claim
            var claims = new List<Claim>
                {
                    new Claim(ClaimsIdentity.DefaultNameClaimType, user.UserName),
                    new Claim(ClaimsIdentity.DefaultRoleClaimType, user.Role)
                };
            // создаем объект ClaimsIdentity
            ClaimsIdentity id = new ClaimsIdentity(claims, "ApplicationCookie", ClaimsIdentity.DefaultNameClaimType,
                ClaimsIdentity.DefaultRoleClaimType);
            // установка аутентификационных куки
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(id));
        }

        [Authorize]
        public async Task<IActionResult> ViewProfile()
        {
            var currentUser = await _context.Users.FirstOrDefaultAsync(u => u.UserName == User.Identity.Name);
            if (currentUser != null)
            {
                var name = currentUser.Name;
                var email = currentUser.Email;
                var sex = currentUser.Sex;
                var phoneNumber = currentUser.PhoneNumber;
                var nationality = currentUser.Nationality;
                var birthday = currentUser.Birthday;
                var countryCode = currentUser.CountryCode;
                var userName = currentUser.UserName;
                var userPhotoUrl = currentUser.UserPhotoUrl;
                ViewBag.UserName = userName;
                if (userPhotoUrl != null)
                {
                    ViewBag.UserPhotoUrl = userPhotoUrl; ;
                }
                else
                {
                    ViewBag.UserPhotoUrl = new byte[10];
                }
                if (birthday != null)
                {
                    ViewBag.DefaultBirthday = birthday.ToString("yyyy-MM-dd"); ;
                }
                else
                {
                    ViewBag.DefaultBirthday = "";
                }
                if (sex != null)
                {
                    ViewBag.DefaultSex = sex;
                }
                else
                {
                    ViewBag.DefaultSex = "";
                }
                if (name != null)
                {
                    ViewBag.DefaultName = name;
                }
                else
                {
                    ViewBag.DefaultName = "";
                }
                if (email != null)
                {
                    ViewBag.DefaultEmail = email;
                }
                else
                {
                    ViewBag.DefaultEmail = "";
                }
                if (phoneNumber != null)
                {
                    ViewBag.DefaultPhoneNumber = phoneNumber;
                }
                else
                {
                    ViewBag.DefaultPhoneNumber = "";
                }
                if (nationality != null)
                {
                    ViewBag.DefaultNationality = nationality;
                }
                else
                {
                    ViewBag.DefaultNationality = "";
                }
                if (countryCode != null)
                {
                    ViewBag.DefaultCountryCode = countryCode;
                }
                else
                {
                    ViewBag.DefaultCountryCode = "";
                }
            }
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SaveProfile([Bind("Name, Email, Sex, CountryCode, PhoneNumber, Nationality, Birthday", Prefix = "model")] User model)
        {
            // Получаем текущего пользователя
            var currentUser = await _context.Users.FirstOrDefaultAsync(u => u.UserName == User.Identity.Name);

            if (currentUser != null)
            {
                // Обновляем данные пользователя
                if (model.Name == null) { model.Name = ""; }
                currentUser.Name = model.Name;
                if (model.Email == null) { model.Email = ""; }
                currentUser.Email = model.Email;
                if (model.Sex == null) { model.Sex = ""; }
                currentUser.Sex = model.Sex;
                if (model.PhoneNumber == null) { model.PhoneNumber = ""; }
                currentUser.PhoneNumber = model.PhoneNumber;
                if (model.CountryCode == null) { model.CountryCode = ""; }
                currentUser.CountryCode = model.CountryCode;
                if (model.Nationality == null) { model.Nationality = ""; }
                currentUser.Nationality = model.Nationality;
                currentUser.Birthday = model.Birthday;

                // Сохраняем изменения в базе данных
                _context.Users.Update(currentUser);
                await _context.SaveChangesAsync();

                // Устанавливаем сообщение об успешном изменении в TempData
                TempData["SuccessMessage"] = "Profile details have been successfully updated.";

                // Перенаправляем пользователя на другую страницу
                return RedirectToAction("ViewProfile", "Account");
            }
            else
            {
                ModelState.AddModelError("", "User not found");
            }

            // Если модель недействительна, возвращаем представление с ошибками
            return View("ViewProfile", model);
        }


        [HttpPost]
        public async Task<IActionResult> SavePhotoProfile()
        {
            var file = Request.Form.Files["file"]; // Получаем файл из запроса

            if (file != null && file.Length > 0)
            {
                using (var memoryStream = new MemoryStream())
                {
                    await file.CopyToAsync(memoryStream);
                    byte[] imageData = memoryStream.ToArray();

                    var currentUser = await _context.Users.FirstOrDefaultAsync(u => u.UserName == User.Identity.Name);
                    // Добавляем изображение в контекст базы данных

                    currentUser.UserPhotoUrl = imageData;

                    // Сохраняем изменения в базе данных
                    _context.Users.Update(currentUser);
                    await _context.SaveChangesAsync();
                }
            }

            // Возвращаем редирект или что-то еще в зависимости от вашей логики
            return RedirectToAction("ViewProfile", "Account");
        }

        [HttpPost]
        public async Task<IActionResult> DeletePhoto()
        {
            var currentUser = await _context.Users.FirstOrDefaultAsync(u => u.UserName == User.Identity.Name);

            if (currentUser != null)
            {
                string filePath = "C:\\Users\\Angelina\\Pictures\\Camera Roll\\Avatar-Profile-No-Background.png";
                byte[] fileBytes = System.IO.File.ReadAllBytes(filePath);
                // Обновляем данные пользователя
                currentUser.UserPhotoUrl = fileBytes;

                // Сохраняем изменения в базе данных
                _context.Users.Update(currentUser);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction("ViewProfile", "Account");
        }

        public async Task<IActionResult> AddObject()
        {
            var entities = new List<EntityModel>
                {
                new EntityModel { Name = "Accommodation", Description = "Description for Accommodation" },
                new EntityModel { Name = "Flight", Description = "Description for Flight" },
                new EntityModel { Name = "Car", Description = "Description for Car" },
                new EntityModel { Name = "Attraction", Description = "Description for Attraction" }
                };

            string filePath = "C:\\Users\\Angelina\\Pictures\\Camera Roll\\bbb846030d108b92a3fefbfca1f7bbe6.jpg";
            byte[] fileBytes = System.IO.File.ReadAllBytes(filePath);
            ViewBag.CarPhoto = fileBytes;

            return View("NewObject", entities);
        }

        [HttpPost]
        public async Task<IActionResult> ClearForm()
        {
            var entities = new List<EntityModel>
                {
                new EntityModel { Name = "Accommodation", Description = "Description for Accommodation" },
                new EntityModel { Name = "Flight", Description = "Description for Flight" },
                new EntityModel { Name = "Car", Description = "Description for Car" },
                new EntityModel { Name = "Attraction", Description = "Description for Attraction" }
                };

            string filePath = "C:\\Users\\Angelina\\Pictures\\Camera Roll\\bbb846030d108b92a3fefbfca1f7bbe6.jpg";
            byte[] fileBytes = System.IO.File.ReadAllBytes(filePath);
            ViewBag.CarPhoto = fileBytes;
            ViewBag.EntityName = "Car";

            return View("NewObject", entities);
        }

        [HttpPost]
        public async Task<IActionResult> SaveForm([Bind("Title, City, Country, Address, IsAirCondition, Transmission, Cost, FreeCancellation, AmountOfPassengers, TheftCoverage, CollisionDamageWaiver, LiabilityCoverage, UnlimitedMileage, CarCategory, ElectricCar, Description, StartDates, EndDates", Prefix = "model")] Car model)
        {
            var currentUser = await _context.Users.FirstOrDefaultAsync(u => u.UserName == User.Identity.Name);

            var entities = new List<EntityModel>
                {
                new EntityModel { Name = "Accommodation", Description = "Description for Accommodation" },
                new EntityModel { Name = "Flight", Description = "Description for Flight" },
                new EntityModel { Name = "Car", Description = "Description for Car" },
                new EntityModel { Name = "Attraction", Description = "Description for Attraction" }
                };

            string filePath = "C:\\Users\\Angelina\\Pictures\\Camera Roll\\bbb846030d108b92a3fefbfca1f7bbe6.jpg";
            byte[] fileBytes = System.IO.File.ReadAllBytes(filePath);

            var file = Request.Form.Files["file"]; // Получаем файл из запроса
            byte[] imageDataa = fileBytes;

            if (file != null && file.Length > 0)
            {
                using (var memoryStream = new MemoryStream())
                {
                    await file.CopyToAsync(memoryStream);
                    byte[] imageData = memoryStream.ToArray();

                    if (imageData == null)
                    {
                        imageData = fileBytes;
                    }
                    ViewBag.CarPhoto = imageData;
                    imageDataa = imageData;
                }
            }
            else { ViewBag.CarPhoto = imageDataa; }

            // Преобразуем модель представления в объект Car и сохраним его в базе данных
            var car = new Car
            {
                Title = model.Title,
                City = model.City,
                Country = model.Country,
                Address = model.Address,
                IsAirCondition = model.IsAirCondition,
                Transmission = model.Transmission,
                Cost = model.Cost,
                CarPhoto = imageDataa,
                FreeCancellation = model.FreeCancellation,
                AmountOfPassengers = model.AmountOfPassengers,
                TheftCoverage = model.TheftCoverage,
                CollisionDamageWaiver = model.CollisionDamageWaiver,
                LiabilityCoverage = model.LiabilityCoverage,
                UnlimitedMileage = model.UnlimitedMileage,
                CarCategory = model.CarCategory,
                ElectricCar = model.ElectricCar,
                Description = model.Description,
                StartDates = model.StartDates,
                EndDates = model.EndDates,
                VerifiedByAdmin = false,
                PublisherId = currentUser.Id,
                RejectedByAdmin = false,
                RejectedMessage = string.Empty
            };
            // добавить сохранение дат в другую БД
            _context.Car.Add(car);

            await _context.SaveChangesAsync();

            // Устанавливаем сообщение об успешном изменении в TempData
            TempData["SuccessMessage"] = "The object Car was successfully saved and sent for moderation.";

            return View("NewObject", entities);

        }

        [HttpPost]
        public async Task<IActionResult> SaveEntertainmentForm(Entertainment model, string photoData)
        {
            var photos = JsonConvert.DeserializeObject<List<string>>(photoData);
            var photoBytesList = photos.Select(photoBase64 => Convert.FromBase64String(photoBase64.Split(',')[1])).ToList();
            var currentUser = await _context.Users.FirstOrDefaultAsync(u => u.UserName == User.Identity.Name);

            var entities = new List<EntityModel>
            {
                new EntityModel { Name = "Accommodation", Description = "Description for Accommodation" },
                new EntityModel { Name = "Flight", Description = "Description for Flight" },
                new EntityModel { Name = "Car", Description = "Description for Car" },
                new EntityModel { Name = "Attraction", Description = "Description for Attraction" }
            };

            var entertainment = new Entertainment
            {
                Title = model.Title,
                City = model.City,
                Country = model.Country,
                Address = model.Address,
                Languages = model.Languages,
                TimeOfDay = model.TimeOfDay,
                Cost = model.Cost,
                FreeCancellation = model.FreeCancellation,
                Category = model.Category,
                MainPhoto = photoBytesList[0],
                Description = model.Description,
                AvailableDates = model.AvailableDates,
                AmountOfTickets = model.AmountOfTickets,
                VerifiedByAdmin = false,
                PublisherId = currentUser.Id, 
                RejectedMessage = string.Empty,
                RejectedByAdmin = false
            };

            string filePath = "C:\\Users\\Angelina\\Pictures\\Camera Roll\\bbb846030d108b92a3fefbfca1f7bbe6.jpg";
            byte[] fileBytes = System.IO.File.ReadAllBytes(filePath);
            ViewBag.CarPhoto = fileBytes;
            // добавить сохранение дат в другую БД
            _context.Entertainment.Add(entertainment);
            await _context.SaveChangesAsync();

            foreach (var photo in photoBytesList)
            {
                var photoss = new Photos
                {
                    ObjectId = entertainment.Id,
                    PhotoArray = photo,
                };
                _context.ObjectPhotos.Add(photoss);
                await _context.SaveChangesAsync();
            }

            // Устанавливаем сообщение об успешном изменении в TempData
            TempData["SuccessMessage"] = "The object Attraction was successfully saved and sent for moderation.";

            return View("NewObject", entities);
        }

        [HttpPost]
        public async Task<IActionResult> SaveAccomodationForm(Accomodation model, string photoDataAccomodation)
        {
            var photos = JsonConvert.DeserializeObject<List<string>>(photoDataAccomodation);
            var photoBytesList = photos.Select(photoBase64 => Convert.FromBase64String(photoBase64.Split(',')[1])).ToList();
            var currentUser = await _context.Users.FirstOrDefaultAsync(u => u.UserName == User.Identity.Name);

            var entities = new List<EntityModel>
            {
                new EntityModel { Name = "Accommodation", Description = "Description for Accommodation" },
                new EntityModel { Name = "Flight", Description = "Description for Flight" },
                new EntityModel { Name = "Car", Description = "Description for Car" },
                new EntityModel { Name = "Attraction", Description = "Description for Attraction" }
            };

            var accommodation = new Accomodation
            {
                Name = model.Name,
                City = model.City,
                Country = model.Country,
                TypesOfNutrition = model.TypesOfNutrition,
                Address = model.Address,
                TypeOfAccomodation = model.TypeOfAccomodation,
                Description = model.Description,
                Parking = model.Parking,
                SwimmingPool = model.SwimmingPool,
                FreeWIFI = model.FreeWIFI,
                PrivateBeach = model.PrivateBeach,
                LineOfBeach = model.LineOfBeach,
                Restaurants = model.Restaurants,
                SPA = model.SPA,
                Bar = model.Bar,
                Garden = model.Garden,
                TransferToAirport = model.TransferToAirport,
                TactileSigns = model.TactileSigns,
                SmookingRooms = model.SmookingRooms,
                FamilyRooms = model.FamilyRooms,
                CarChargingStation = model.CarChargingStation,
                WheelchairAccessible = model.WheelchairAccessible,
                FitnessCentre = model.FitnessCentre,
                PetsAllowed = model.PetsAllowed,
                DeliveryFoodToTheRoom = model.DeliveryFoodToTheRoom,
                EveryHourFrontDesk = model.EveryHourFrontDesk,
                MainPhoto = photoBytesList[0],
                VerifiedByAdmin = false,  
                RejectedByAdmin = false,
                PublisherId = currentUser.Id,
                MinCost = 0,
                RejectedMessage = string.Empty
            };

            string filePath = "C:\\Users\\Angelina\\Pictures\\Camera Roll\\bbb846030d108b92a3fefbfca1f7bbe6.jpg";
            byte[] fileBytes = System.IO.File.ReadAllBytes(filePath);
            ViewBag.CarPhoto = fileBytes;
            // добавить сохранение дат в другую БД
            _context.Accomodation.Add(accommodation);
            await _context.SaveChangesAsync();

            foreach (var photo in photoBytesList)
            {
                var photoss = new AccomodationPhotos
                {
                    ObjectId = accommodation.Id,
                    PhotoArray = photo,
                };
                _context.AccomodationPhotos.Add(photoss);
                await _context.SaveChangesAsync();
            }

            // Устанавливаем сообщение об успешном изменении в TempData
            TempData["SuccessMessage"] = "The object Accommodation was successfully saved and sent for moderation.";

            return View("NewObject", entities);
        }

        [HttpPost]
        public async Task<IActionResult> SaveFlightForm([Bind("CityFrom, CountryFrom, CityTo, CountryTo, FlightNumber, CostEC, CostBC, CostFC, ArrivalTime, DepartureTime, AmountOfTicketsEC, AmountOfTicketsBC, AmountOfTicketsFC, FreeCancellation, IncludeLuggageEC, IncludeLuggageBC, IncludeLuggageFC, ArrivalCountryCode, DepartureCountryCode", Prefix = "model")] AirTicket model)
        {
            var currentUser = await _context.Users.FirstOrDefaultAsync(u => u.UserName == User.Identity.Name);

            var entities = new List<EntityModel>
                {
                new EntityModel { Name = "Accommodation", Description = "Description for Accommodation" },
                new EntityModel { Name = "Flight", Description = "Description for Flight" },
                new EntityModel { Name = "Car", Description = "Description for Car" },
                new EntityModel { Name = "Attraction", Description = "Description for Attraction" }
                };

            var airticket = new AirTicket
            {
                CityFrom = model.CityFrom,
                CountryFrom = model.CountryFrom,
                CityTo = model.CityTo,
                CountryTo = model.CountryTo,
                FlightNumber = model.FlightNumber,
                CostEC = model.CostEC,
                CostBC = model.CostBC,
                CostFC = model.CostFC,
                ArrivalTime = model.ArrivalTime,
                DepartureTime = model.DepartureTime,
                AmountOfTicketsEC = model.AmountOfTicketsEC,
                AmountOfTicketsBC = model.AmountOfTicketsBC,
                AmountOfTicketsFC = model.AmountOfTicketsFC,
                FreeCancellation = model.FreeCancellation,
                IncludeLuggageEC = model.IncludeLuggageEC,
                IncludeLuggageBC = model.IncludeLuggageBC,
                IncludeLuggageFC = model.IncludeLuggageFC,
                ArrivalCountryCode = model.ArrivalCountryCode,
                DepartureCountryCode = model.DepartureCountryCode,
                VerifiedByAdminECTicket = false,
                VerifiedByAdminBCTicket = false,
                VerifiedByAdminFCTicket = false,
                PublisherId = currentUser.Id,
                RejectedByAdminBC = false,
                RejectedByAdminEC = false,
                RejectedByAdminFC = false,
                RejectedMessageEC = string.Empty,
                RejectedMessageBC = string.Empty,
                RejectedMessageFC = string.Empty
            };

            string filePath = "C:\\Users\\Angelina\\Pictures\\Camera Roll\\bbb846030d108b92a3fefbfca1f7bbe6.jpg";
            byte[] fileBytes = System.IO.File.ReadAllBytes(filePath);
            ViewBag.CarPhoto = fileBytes;
            // добавить сохранение дат в другую БД
            _context.AirTicket.Add(airticket);
            await _context.SaveChangesAsync();

            // Устанавливаем сообщение об успешном изменении в TempData
            TempData["SuccessMessage"] = "The object AirTicket was successfully saved and sent for moderation.";

            return View("NewObject", entities);
        }

        public async Task<IActionResult> UnverifiedObjects()
        {
            var currentUser = await _context.Users.FirstOrDefaultAsync(u => u.UserName == User.Identity.Name);

            var unverifiedCars = await _context.Car
            .Where(c => !c.VerifiedByAdmin && !c.RejectedByAdmin && c.PublisherId == currentUser.Id)
            .ToListAsync();

            var unverifiedAirTickets = await _context.AirTicket
           .Where(c => ((!c.VerifiedByAdminECTicket && !c.RejectedByAdminEC) || (!c.VerifiedByAdminBCTicket && !c.RejectedByAdminBC) || (!c.VerifiedByAdminFCTicket && !c.RejectedByAdminFC)) && c.PublisherId == currentUser.Id)
           .ToListAsync();

            var unverifiedAttractions = await _context.Entertainment
          .Where(c => !c.VerifiedByAdmin && !c.RejectedByAdmin && c.PublisherId == currentUser.Id)
          .ToListAsync();

            var unverifiedAccomodation = await _context.Accomodation
         .Where(c => !c.VerifiedByAdmin && !c.RejectedByAdmin && c.PublisherId == currentUser.Id)
         .ToListAsync();

            var model = new UnverifiedObjectsViewModel
            {
                UnverifiedCars = unverifiedCars,
                UnverifiedAirTickets = unverifiedAirTickets,
                UnverifiedAttractions = unverifiedAttractions,
                UnverifiedAccomodation = unverifiedAccomodation
            };

            return View("UnverifiedItems", model);
        }

        public async Task<IActionResult> VerifiedObjects()
        {
            var currentUser = await _context.Users.FirstOrDefaultAsync(u => u.UserName == User.Identity.Name);

            var verifiedCars = await _context.Car
            .Where(c => c.VerifiedByAdmin && c.PublisherId == currentUser.Id)
            .ToListAsync();

            var verifiedAirTickets = await _context.AirTicket
           .Where(c => (c.VerifiedByAdminECTicket || c.VerifiedByAdminBCTicket || c.VerifiedByAdminFCTicket) && c.PublisherId == currentUser.Id)
           .ToListAsync();

            var verifiedAttractions = await _context.Entertainment
           .Where(c => c.VerifiedByAdmin && c.PublisherId == currentUser.Id)
           .ToListAsync();

            var verifiedAccomodation = await _context.Accomodation
           .Where(c => c.VerifiedByAdmin && c.PublisherId == currentUser.Id)
           .ToListAsync();

            var model = new VerifiedObjectsViewModel
            {
                VerifiedCars = verifiedCars,
                VerifiedAirTickets = verifiedAirTickets,
                VerifiedAttractions = verifiedAttractions,
                VerifiedAccomodation = verifiedAccomodation
            };

            return View("VerifiedItems", model);
        }

        public async Task<IActionResult> RejectedObjects()
        {
            var currentUser = await _context.Users.FirstOrDefaultAsync(u => u.UserName == User.Identity.Name);

            var unverifiedCars = await _context.Car
            .Where(c => !c.VerifiedByAdmin && c.RejectedByAdmin && c.PublisherId == currentUser.Id)
            .ToListAsync();

            var unverifiedAirTickets = await _context.AirTicket
           .Where(c => ((!c.VerifiedByAdminECTicket && c.RejectedByAdminEC) || (!c.VerifiedByAdminBCTicket && c.RejectedByAdminBC) || (!c.VerifiedByAdminFCTicket && c.RejectedByAdminFC)) && c.PublisherId == currentUser.Id)
           .ToListAsync();

            var unverifiedAttractions = await _context.Entertainment
            .Where(c => !c.VerifiedByAdmin && c.RejectedByAdmin && c.PublisherId == currentUser.Id)
            .ToListAsync();

            var unverifiedAccomodation = await _context.Accomodation
          .Where(c => !c.VerifiedByAdmin && c.RejectedByAdmin && c.PublisherId == currentUser.Id)
          .ToListAsync();

            var model = new RejectedObjectsViewModel
            {
                RejectedCars = unverifiedCars,
                RejectedAirTickets = unverifiedAirTickets,
                RejectedAttractions = unverifiedAttractions,
                RejectedAccomodation = unverifiedAccomodation
            };

            return View("RejectedItems", model);
        }

        public async Task<IActionResult> BookingsForVerification()
        {
            var currentUser = await _context.Users.FirstOrDefaultAsync(u => u.UserName == User.Identity.Name);


            var unverifiedCars = await _context.BookingCars
            .Where(c => !c.VerifiedBooking && !c.RejectedBooking && !c.CanceledBooking)
            .ToListAsync();

            var unverifiedAirTickets = await _context.BookingAirTickets
           .Where(c => !c.VerifiedBooking && !c.RejectedBooking && !c.CanceledBooking)
           .ToListAsync();

            var unverifiedAttractions = await _context.BookingAttractions
            .Where(c => !c.VerifiedBooking && !c.RejectedBooking && !c.CanceledBooking)
            .ToListAsync();

            var unverifiedAccomodation = await _context.BookingAccomodations
              .Where(c => !c.VerifiedBooking && !c.RejectedBooking && !c.CanceledBooking)
              .ToListAsync();

            var myUnverifiedCars = new List<BookingCar>();
            var myUnverifiedAirTickets = new List<BookingAirTicket>();
            var myUnverifiedAttractions = new List<BookingAttraction>();
            var myUnverifiedAccomodation = new List<BookingAccomodation>();

            foreach (var car in unverifiedCars)
            {
                var exists = _context.Car.Any(c => c.Id == car.CarId && c.PublisherId == currentUser.Id);

                if (exists)
                {
                    // Add to the verifiedCars list
                    myUnverifiedCars.Add(car);
                }
            }

            foreach (var airticket in unverifiedAirTickets)
            {
                var exists = _context.AirTicket.Any(c => c.Id == airticket.AirTicketId && c.PublisherId == currentUser.Id);

                if (exists)
                {
                    // Add to the verifiedCars list
                    myUnverifiedAirTickets.Add(airticket);
                }
            }

            foreach (var attraction in unverifiedAttractions)
            {
                var exists = _context.Entertainment.Any(c => c.Id == attraction.AttractionId && c.PublisherId == currentUser.Id);

                if (exists)
                {
                    // Add to the verifiedCars list
                    myUnverifiedAttractions.Add(attraction);
                }
            }

            foreach (var accomodation in unverifiedAccomodation)
            {
                var exists = _context.Accomodation.Any(c => c.Id == accomodation.AccomodationId && c.PublisherId == currentUser.Id);

                if (exists)
                {
                    // Add to the verifiedCars list
                    myUnverifiedAccomodation.Add(accomodation);
                }
            }

            var cars = new List<Car>();
            var usersDataCars = new List<UserData>();

            foreach (var car in myUnverifiedCars)
            {
                var currentCar = _context.Car.Find(car.CarId);
                cars.Add(currentCar);
                var user = _context.Users.Find(car.UserId);
                var userData = new UserData();
                userData.Name = user.Name;
                userData.Email = user.Email;
                userData.PhoneNumber = user.CountryCode + user.PhoneNumber;
                usersDataCars.Add(userData);
            }

            var airtickets = new List<AirTicket>();
            var usersDataAirTickets = new List<UserData>();

            foreach (var airticket in myUnverifiedAirTickets)
            {
                var currentAirticket = _context.AirTicket.Find(airticket.AirTicketId);
                airtickets.Add(currentAirticket);
                var user = _context.Users.Find(airticket.UserId);
                var userData = new UserData();
                userData.Name = user.Name;
                userData.Email = user.Email;
                userData.PhoneNumber = user.CountryCode + user.PhoneNumber;
                usersDataAirTickets.Add(userData);
            }

            var attractions = new List<Entertainment>();
            var usersDataAttractions = new List<UserData>();

            foreach (var attraction in myUnverifiedAttractions)
            {
                var currentAttraction = _context.Entertainment.Find(attraction.AttractionId);
                attractions.Add(currentAttraction);
                var user = _context.Users.Find(attraction.UserId);
                var userData = new UserData();
                userData.Name = user.Name;
                userData.Email = user.Email;
                userData.PhoneNumber = user.CountryCode + user.PhoneNumber;
                usersDataAttractions.Add(userData);
            }

            var accomodations = new List<Accomodation>();
            var usersDataAccomodation = new List<UserData>();

            foreach (var accomodation in myUnverifiedAccomodation)
            {
                var currentAccomodation = _context.Accomodation.Find(accomodation.AccomodationId);
                accomodations.Add(currentAccomodation);
                var user = _context.Users.Find(accomodation.UserId);
                var userData = new UserData();
                userData.Name = user.Name;
                userData.Email = user.Email;
                userData.PhoneNumber = user.CountryCode + user.PhoneNumber;
                usersDataAccomodation.Add(userData);
            }

            

            var model = new ForVerificationObjectsViewModel
            {
                Cars = cars,
                AirTickets = airtickets,
                Attractions = attractions,
                Accomodation = accomodations,
                BookingCars = myUnverifiedCars,
                BookingAirTickets = myUnverifiedAirTickets,
                BookingAttractions = myUnverifiedAttractions,
                BookingAccomodation = myUnverifiedAccomodation,
                UsersDataAttractions = usersDataAttractions,
                UsersDataAirTickets = usersDataAirTickets,
                UsersDataAccomodation = usersDataAccomodation,
                UsersDataCars = usersDataCars
            };

            return View("BookingsForVerification", model);
        }

        public async Task<IActionResult> ShowBookingItems()
        {
            var currentUser = await _context.Users.FirstOrDefaultAsync(u => u.UserName == User.Identity.Name);


            var unverifiedCars = await _context.BookingCars
            .Where(c => c.VerifiedBooking && c.UserId == currentUser.Id && !c.CanceledBooking)
            .ToListAsync();

            var unverifiedAirTickets = await _context.BookingAirTickets
           .Where(c => c.VerifiedBooking && c.UserId == currentUser.Id && !c.CanceledBooking)
           .ToListAsync();

            var unverifiedAttractions = await _context.BookingAttractions
            .Where(c => c.VerifiedBooking && c.UserId == currentUser.Id && !c.CanceledBooking)
            .ToListAsync();

            var unverifiedAccomodation = await _context.BookingAccomodations
              .Where(c => c.VerifiedBooking && c.UserId == currentUser.Id && !c.CanceledBooking)
              .ToListAsync();

            var myUnverifiedCars = new List<BookingCar>();
            var myUnverifiedAirTickets = new List<BookingAirTicket>();
            var myUnverifiedAttractions = new List<BookingAttraction>();
            var myUnverifiedAccomodation = new List<BookingAccomodation>();

            foreach (var car in unverifiedCars)
            {
                var exists = _context.Car.Any(c => c.Id == car.CarId);

                if (exists)
                {
                    // Add to the verifiedCars list
                    myUnverifiedCars.Add(car);
                }
            }

            foreach (var airticket in unverifiedAirTickets)
            {
                var exists = _context.AirTicket.Any(c => c.Id == airticket.AirTicketId);

                if (exists)
                {
                    // Add to the verifiedCars list
                    myUnverifiedAirTickets.Add(airticket);
                }
            }

            foreach (var attraction in unverifiedAttractions)
            {
                var exists = _context.Entertainment.Any(c => c.Id == attraction.AttractionId);

                if (exists)
                {
                    // Add to the verifiedCars list
                    myUnverifiedAttractions.Add(attraction);
                }
            }

            foreach (var accomodation in unverifiedAccomodation)
            {
                var exists = _context.Accomodation.Any(c => c.Id == accomodation.AccomodationId);

                if (exists)
                {
                    // Add to the verifiedCars list
                    myUnverifiedAccomodation.Add(accomodation);
                }
            }

            var cars = new List<Car>();

            foreach (var car in myUnverifiedCars)
            {
                var currentCar = _context.Car.Find(car.CarId);
                cars.Add(currentCar);
            }

            var airtickets = new List<AirTicket>();

            foreach (var airticket in myUnverifiedAirTickets)
            {
                var currentAirticket = _context.AirTicket.Find(airticket.AirTicketId);
                airtickets.Add(currentAirticket);
            }

            var attractions = new List<Entertainment>();

            foreach (var attraction in myUnverifiedAttractions)
            {
                var currentAttraction = _context.Entertainment.Find(attraction.AttractionId);
                attractions.Add(currentAttraction);
            }

            var accomodations = new List<Accomodation>();

            foreach (var accomodation in myUnverifiedAccomodation)
            {
                var currentAccomodation = _context.Accomodation.Find(accomodation.AccomodationId);
                accomodations.Add(currentAccomodation);
            }

            var model = new WaitingBookingsViewModel
            {
                Cars = cars,
                AirTickets = airtickets,
                Attractions = attractions,
                Accomodation = accomodations,
                BookingCars = myUnverifiedCars,
                BookingAirTickets = myUnverifiedAirTickets,
                BookingAttractions = myUnverifiedAttractions,
                BookingAccomodation = myUnverifiedAccomodation
            };

            return View("BookingItems", model);
        }

        public async Task<IActionResult> ShowUnverifiedBookingItems()
        {
            var currentUser = await _context.Users.FirstOrDefaultAsync(u => u.UserName == User.Identity.Name);


            var unverifiedCars = await _context.BookingCars
            .Where(c => !c.VerifiedBooking && !c.RejectedBooking && c.UserId == currentUser.Id && !c.CanceledBooking)
            .ToListAsync();

            var unverifiedAirTickets = await _context.BookingAirTickets
           .Where(c => !c.VerifiedBooking && !c.RejectedBooking && c.UserId == currentUser.Id && !c.CanceledBooking)
           .ToListAsync();

            var unverifiedAttractions = await _context.BookingAttractions
            .Where(c => !c.VerifiedBooking && !c.RejectedBooking && c.UserId == currentUser.Id && !c.CanceledBooking)
            .ToListAsync();

            var unverifiedAccomodation = await _context.BookingAccomodations
              .Where(c => !c.VerifiedBooking && !c.RejectedBooking && c.UserId == currentUser.Id && !c.CanceledBooking)
              .ToListAsync();

            var myUnverifiedCars = new List<BookingCar>();
            var myUnverifiedAirTickets = new List<BookingAirTicket>();
            var myUnverifiedAttractions = new List<BookingAttraction>();
            var myUnverifiedAccomodation = new List<BookingAccomodation>();

            foreach (var car in unverifiedCars)
            {
                var exists = _context.Car.Any(c => c.Id == car.CarId);

                if (exists)
                {
                    // Add to the verifiedCars list
                    myUnverifiedCars.Add(car);
                }
            }

            foreach (var airticket in unverifiedAirTickets)
            {
                var exists = _context.AirTicket.Any(c => c.Id == airticket.AirTicketId);

                if (exists)
                {
                    // Add to the verifiedCars list
                    myUnverifiedAirTickets.Add(airticket);
                }
            }

            foreach (var attraction in unverifiedAttractions)
            {
                var exists = _context.Entertainment.Any(c => c.Id == attraction.AttractionId);

                if (exists)
                {
                    // Add to the verifiedCars list
                    myUnverifiedAttractions.Add(attraction);
                }
            }

            foreach (var accomodation in unverifiedAccomodation)
            {
                var exists = _context.Accomodation.Any(c => c.Id == accomodation.AccomodationId);

                if (exists)
                {
                    // Add to the verifiedCars list
                    myUnverifiedAccomodation.Add(accomodation);
                }
            }

            var cars = new List<Car>();

            foreach (var car in myUnverifiedCars)
            {
                var currentCar = _context.Car.Find(car.CarId);
                cars.Add(currentCar);
            }

            var airtickets = new List<AirTicket>();

            foreach (var airticket in myUnverifiedAirTickets)
            {
                var currentAirticket = _context.AirTicket.Find(airticket.AirTicketId);
                airtickets.Add(currentAirticket);
            }

            var attractions = new List<Entertainment>();

            foreach (var attraction in myUnverifiedAttractions)
            {
                var currentAttraction = _context.Entertainment.Find(attraction.AttractionId);
                attractions.Add(currentAttraction);
            }

            var accomodations = new List<Accomodation>();

            foreach (var accomodation in myUnverifiedAccomodation)
            {
                var currentAccomodation = _context.Accomodation.Find(accomodation.AccomodationId);
                accomodations.Add(currentAccomodation);
            }

            var model = new WaitingBookingsViewModel
            {
                Cars = cars,
                AirTickets = airtickets,
                Attractions = attractions,
                Accomodation = accomodations,
                BookingCars = myUnverifiedCars,
                BookingAirTickets = myUnverifiedAirTickets,
                BookingAttractions = myUnverifiedAttractions,
                BookingAccomodation = myUnverifiedAccomodation
            };

            return View("AwaitingBookings", model);
        }

        public async Task<IActionResult> VerifiedBookings()
        {
            var currentUser = await _context.Users.FirstOrDefaultAsync(u => u.UserName == User.Identity.Name);


            var unverifiedCars = await _context.BookingCars
            .Where(c => c.VerifiedBooking && !c.CanceledBooking)
            .ToListAsync();

            var unverifiedAirTickets = await _context.BookingAirTickets
           .Where(c => c.VerifiedBooking && !c.CanceledBooking)
           .ToListAsync();

            var unverifiedAttractions = await _context.BookingAttractions
            .Where(c => c.VerifiedBooking && !c.CanceledBooking)
            .ToListAsync();

            var unverifiedAccomodation = await _context.BookingAccomodations
              .Where(c => c.VerifiedBooking && !c.CanceledBooking)
              .ToListAsync();

            var myVerifiedCars = new List<BookingCar>();
            var myVerifiedAirTickets = new List<BookingAirTicket>();
            var myVerifiedAttractions = new List<BookingAttraction>();
            var myVerifiedAccomodation = new List<BookingAccomodation>();

            foreach (var car in unverifiedCars)
            {
                var exists = _context.Car.Any(c => c.Id == car.CarId && c.PublisherId == currentUser.Id);

                if (exists)
                {
                    // Add to the verifiedCars list
                    myVerifiedCars.Add(car);
                }
            }

            foreach (var airticket in unverifiedAirTickets)
            {
                var exists = _context.AirTicket.Any(c => c.Id == airticket.AirTicketId && c.PublisherId == currentUser.Id);

                if (exists)
                {
                    // Add to the verifiedCars list
                    myVerifiedAirTickets.Add(airticket);
                }
            }

            foreach (var attraction in unverifiedAttractions)
            {
                var exists = _context.Entertainment.Any(c => c.Id == attraction.AttractionId && c.PublisherId == currentUser.Id);

                if (exists)
                {
                    // Add to the verifiedCars list
                    myVerifiedAttractions.Add(attraction);
                }
            }

            foreach (var accomodation in unverifiedAccomodation)
            {
                var exists = _context.Accomodation.Any(c => c.Id == accomodation.AccomodationId && c.PublisherId == currentUser.Id);

                if (exists)
                {
                    // Add to the verifiedCars list
                    myVerifiedAccomodation.Add(accomodation);
                }
            }

            var cars = new List<Car>();
            var usersDataCars = new List<UserData>();

            foreach (var car in myVerifiedCars)
            {
                var currentCar = _context.Car.Find(car.CarId);
                cars.Add(currentCar);
                var user = _context.Users.Find(car.UserId);
                var userData = new UserData();
                userData.Name = user.Name;
                userData.Email = user.Email;
                userData.PhoneNumber = user.CountryCode + user.PhoneNumber;
                usersDataCars.Add(userData);
            }

            var airtickets = new List<AirTicket>();
            var usersDataAirTickets = new List<UserData>();

            foreach (var airticket in myVerifiedAirTickets)
            {
                var currentAirticket = _context.AirTicket.Find(airticket.AirTicketId);
                airtickets.Add(currentAirticket);
                var user = _context.Users.Find(airticket.UserId);
                var userData = new UserData();
                userData.Name = user.Name;
                userData.Email = user.Email;
                userData.PhoneNumber = user.CountryCode + user.PhoneNumber;
                usersDataAirTickets.Add(userData);
            }

            var attractions = new List<Entertainment>();
            var usersDataAttractions = new List<UserData>();

            foreach (var attraction in myVerifiedAttractions)
            {
                var currentAttraction = _context.Entertainment.Find(attraction.AttractionId);
                attractions.Add(currentAttraction);
                var user = _context.Users.Find(attraction.UserId);
                var userData = new UserData();
                userData.Name = user.Name;
                userData.Email = user.Email;
                userData.PhoneNumber = user.CountryCode + user.PhoneNumber;
                usersDataAttractions.Add(userData);
            }

            var accomodations = new List<Accomodation>();
            var usersDataAccomodation = new List<UserData>();

            foreach (var accomodation in myVerifiedAccomodation)
            {
                var currentAccomodation = _context.Accomodation.Find(accomodation.AccomodationId);
                accomodations.Add(currentAccomodation);
                var user = _context.Users.Find(accomodation.UserId);
                var userData = new UserData();
                userData.Name = user.Name;
                userData.Email = user.Email;
                userData.PhoneNumber = user.CountryCode + user.PhoneNumber;
                usersDataAccomodation.Add(userData);
            }

            var model = new ForVerificationObjectsViewModel
            {
                Cars = cars,
                AirTickets = airtickets,
                Attractions = attractions,
                Accomodation = accomodations,
                BookingCars = myVerifiedCars,
                BookingAirTickets = myVerifiedAirTickets,
                BookingAttractions = myVerifiedAttractions,
                BookingAccomodation = myVerifiedAccomodation,
                UsersDataAttractions = usersDataAttractions,
                UsersDataAirTickets = usersDataAirTickets,
                UsersDataAccomodation = usersDataAccomodation,
                UsersDataCars = usersDataCars
            };

            return View("VerifiedBookingsOwner", model);
        }

        public async Task<IActionResult> RejectedBookings()
        {
            var currentUser = await _context.Users.FirstOrDefaultAsync(u => u.UserName == User.Identity.Name);


            var unverifiedCars = await _context.BookingCars
            .Where(c => c.RejectedBooking)
            .ToListAsync();

            var unverifiedAirTickets = await _context.BookingAirTickets
           .Where(c => c.RejectedBooking)
           .ToListAsync();

            var unverifiedAttractions = await _context.BookingAttractions
            .Where(c => c.RejectedBooking)
            .ToListAsync();

            var unverifiedAccomodation = await _context.BookingAccomodations
              .Where(c => c.RejectedBooking)
              .ToListAsync();

            var myVerifiedCars = new List<BookingCar>();
            var myVerifiedAirTickets = new List<BookingAirTicket>();
            var myVerifiedAttractions = new List<BookingAttraction>();
            var myVerifiedAccomodation = new List<BookingAccomodation>();

            foreach (var car in unverifiedCars)
            {
                var exists = _context.Car.Any(c => c.Id == car.CarId && c.PublisherId == currentUser.Id);

                if (exists)
                {
                    // Add to the verifiedCars list
                    myVerifiedCars.Add(car);
                }
            }

            foreach (var airticket in unverifiedAirTickets)
            {
                var exists = _context.AirTicket.Any(c => c.Id == airticket.AirTicketId && c.PublisherId == currentUser.Id);

                if (exists)
                {
                    // Add to the verifiedCars list
                    myVerifiedAirTickets.Add(airticket);
                }
            }

            foreach (var attraction in unverifiedAttractions)
            {
                var exists = _context.Entertainment.Any(c => c.Id == attraction.AttractionId && c.PublisherId == currentUser.Id);

                if (exists)
                {
                    // Add to the verifiedCars list
                    myVerifiedAttractions.Add(attraction);
                }
            }

            foreach (var accomodation in unverifiedAccomodation)
            {
                var exists = _context.Accomodation.Any(c => c.Id == accomodation.AccomodationId && c.PublisherId == currentUser.Id);

                if (exists)
                {
                    // Add to the verifiedCars list
                    myVerifiedAccomodation.Add(accomodation);
                }
            }

            var cars = new List<Car>();
            var usersDataCars = new List<UserData>();
            var carMessages = new List<string>();

            foreach (var car in myVerifiedCars)
            {
                var currentCar = _context.Car.Find(car.CarId);
                cars.Add(currentCar);
                var user = _context.Users.Find(car.UserId);
                var userData = new UserData();
                userData.Name = user.Name;
                userData.Email = user.Email;
                userData.PhoneNumber = user.CountryCode + user.PhoneNumber;
                usersDataCars.Add(userData);
                carMessages.Add(car.RejectedMessage);
            }

            var airtickets = new List<AirTicket>();
            var usersDataAirTickets = new List<UserData>();
            var airticketMessages = new List<string>();

            foreach (var airticket in myVerifiedAirTickets)
            {
                var currentAirticket = _context.AirTicket.Find(airticket.AirTicketId);
                airtickets.Add(currentAirticket);
                var user = _context.Users.Find(airticket.UserId);
                var userData = new UserData();
                userData.Name = user.Name;
                userData.Email = user.Email;
                userData.PhoneNumber = user.CountryCode + user.PhoneNumber;
                usersDataAirTickets.Add(userData);
                airticketMessages.Add(airticket.RejectedMessage);
            }

            var attractions = new List<Entertainment>();
            var usersDataAttractions = new List<UserData>();
            var attractionMessages = new List<string>();

            foreach (var attraction in myVerifiedAttractions)
            {
                var currentAttraction = _context.Entertainment.Find(attraction.AttractionId);
                attractions.Add(currentAttraction);
                var user = _context.Users.Find(attraction.UserId);
                var userData = new UserData();
                userData.Name = user.Name;
                userData.Email = user.Email;
                userData.PhoneNumber = user.CountryCode + user.PhoneNumber;
                usersDataAttractions.Add(userData);
                attractionMessages.Add(attraction.RejectedMessage);
            }

            var accomodations = new List<Accomodation>();
            var usersDataAccomodation = new List<UserData>();
            var accomodationMessages = new List<string>();

            foreach (var accomodation in myVerifiedAccomodation)
            {
                var currentAccomodation = _context.Accomodation.Find(accomodation.AccomodationId);
                accomodations.Add(currentAccomodation);
                var user = _context.Users.Find(accomodation.UserId);
                var userData = new UserData();
                userData.Name = user.Name;
                userData.Email = user.Email;
                userData.PhoneNumber = user.CountryCode + user.PhoneNumber;
                usersDataAccomodation.Add(userData);
                accomodationMessages.Add(accomodation.RejectedMessage);
            }

            var model = new ForRejectedObjectsViewModel
            {
                Cars = cars,
                AirTickets = airtickets,
                Attractions = attractions,
                Accomodation = accomodations,
                BookingCars = myVerifiedCars,
                BookingAirTickets = myVerifiedAirTickets,
                BookingAttractions = myVerifiedAttractions,
                BookingAccomodation = myVerifiedAccomodation,
                UsersDataAttractions = usersDataAttractions,
                UsersDataAirTickets = usersDataAirTickets,
                UsersDataAccomodation = usersDataAccomodation,
                UsersDataCars = usersDataCars, 
                CarMessages = carMessages,
                AirticketMessages = airticketMessages,
                AttractionMessages = attractionMessages,
                AccomodationMessages = accomodationMessages
            };

            return View("RejectedBookingsOwner", model);
        }

        public async Task<IActionResult> ShowRejectedBookingItems()
        {
            var currentUser = await _context.Users.FirstOrDefaultAsync(u => u.UserName == User.Identity.Name);


            var unverifiedCars = await _context.BookingCars
            .Where(c => c.RejectedBooking && c.UserId == currentUser.Id)
            .ToListAsync();

            var unverifiedAirTickets = await _context.BookingAirTickets
           .Where(c => c.RejectedBooking && c.UserId == currentUser.Id)
           .ToListAsync();

            var unverifiedAttractions = await _context.BookingAttractions
            .Where(c => c.RejectedBooking && c.UserId == currentUser.Id)
            .ToListAsync();

            var unverifiedAccomodation = await _context.BookingAccomodations
              .Where(c => c.RejectedBooking && c.UserId == currentUser.Id)
              .ToListAsync();

            var myVerifiedCars = new List<BookingCar>();
            var myVerifiedAirTickets = new List<BookingAirTicket>();
            var myVerifiedAttractions = new List<BookingAttraction>();
            var myVerifiedAccomodation = new List<BookingAccomodation>();

            foreach (var car in unverifiedCars)
            {
                var exists = _context.Car.Any(c => c.Id == car.CarId);

                if (exists)
                {
                    // Add to the verifiedCars list
                    myVerifiedCars.Add(car);
                }
            }

            foreach (var airticket in unverifiedAirTickets)
            {
                var exists = _context.AirTicket.Any(c => c.Id == airticket.AirTicketId);

                if (exists)
                {
                    // Add to the verifiedCars list
                    myVerifiedAirTickets.Add(airticket);
                }
            }

            foreach (var attraction in unverifiedAttractions)
            {
                var exists = _context.Entertainment.Any(c => c.Id == attraction.AttractionId);

                if (exists)
                {
                    // Add to the verifiedCars list
                    myVerifiedAttractions.Add(attraction);
                }
            }

            foreach (var accomodation in unverifiedAccomodation)
            {
                var exists = _context.Accomodation.Any(c => c.Id == accomodation.AccomodationId);

                if (exists)
                {
                    // Add to the verifiedCars list
                    myVerifiedAccomodation.Add(accomodation);
                }
            }

            var cars = new List<Car>();
            var carMessages = new List<string>();

            foreach (var car in myVerifiedCars)
            {
                var currentCar = _context.Car.Find(car.CarId);
                cars.Add(currentCar);
                carMessages.Add(car.RejectedMessage);
            }

            var airtickets = new List<AirTicket>();
            var airticketMessages = new List<string>();

            foreach (var airticket in myVerifiedAirTickets)
            {
                var currentAirticket = _context.AirTicket.Find(airticket.AirTicketId);
                airtickets.Add(currentAirticket);
                airticketMessages.Add(airticket.RejectedMessage);
            }

            var attractions = new List<Entertainment>();
            var attractionMessages = new List<string>();

            foreach (var attraction in myVerifiedAttractions)
            {
                var currentAttraction = _context.Entertainment.Find(attraction.AttractionId);
                attractions.Add(currentAttraction);
                attractionMessages.Add(attraction.RejectedMessage);
            }

            var accomodations = new List<Accomodation>();
            var accomodationMessages = new List<string>();

            foreach (var accomodation in myVerifiedAccomodation)
            {
                var currentAccomodation = _context.Accomodation.Find(accomodation.AccomodationId);
                accomodations.Add(currentAccomodation);
                accomodationMessages.Add(accomodation.RejectedMessage);
            }

            var model = new ForRejectedMyBookingsObjectsViewModel
            {
                Cars = cars,
                AirTickets = airtickets,
                Attractions = attractions,
                Accomodation = accomodations,
                BookingCars = myVerifiedCars,
                BookingAirTickets = myVerifiedAirTickets,
                BookingAttractions = myVerifiedAttractions,
                BookingAccomodation = myVerifiedAccomodation,
                CarMessages = carMessages,
                AirticketMessages = airticketMessages,
                AttractionMessages = attractionMessages,
                AccomodationMessages = accomodationMessages
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteCarUnverified(int deleteCarId)
        {
            var car = _context.Car.Find(deleteCarId); // Находим автомобиль по его ID

            if (car != null)
            {
                _context.Car.Remove(car); // Удаляем автомобиль из контекста
                _context.SaveChanges(); // Сохраняем изменения в базе данных
            }

            return await UnverifiedObjects();
        }

        [HttpPost]
        public async Task<IActionResult> DeleteAttractionUnverified(int deleteAttractionId)
        {
            var attraction = _context.Entertainment.Find(deleteAttractionId);
            var objectsToDelete = _context.ObjectPhotos.Where(o => o.ObjectId == deleteAttractionId).ToList();

            if (objectsToDelete.Any())
            {
                // Удаляем найденные объекты
                _context.ObjectPhotos.RemoveRange(objectsToDelete);
                await _context.SaveChangesAsync();
            }

            if (attraction != null)
            {
                _context.Entertainment.Remove(attraction); // Удаляем автомобиль из контекста
                _context.SaveChanges(); // Сохраняем изменения в базе данных
            }

            return await UnverifiedObjects();
        }

        [HttpPost]
        public async Task<IActionResult> DeleteAccomodationUnverified(int deleteAccomodationId)
        {
            var accomodation = _context.Accomodation.Find(deleteAccomodationId);
            var objectsToDelete = _context.AccomodationPhotos.Where(o => o.ObjectId == deleteAccomodationId).ToList();

            if (objectsToDelete.Any())
            {
                // Удаляем найденные объекты
                _context.AccomodationPhotos.RemoveRange(objectsToDelete);
                await _context.SaveChangesAsync();
            }

            if (accomodation != null)
            {
                _context.Accomodation.Remove(accomodation); // Удаляем автомобиль из контекста
                _context.SaveChanges(); // Сохраняем изменения в базе данных
            }

            return await UnverifiedObjects();
        }

        [HttpPost]
        public async Task<IActionResult> DeleteCarVerified(int deleteCarId)
        {
            var car = _context.Car.Find(deleteCarId); // Находим автомобиль по его ID

            if (car != null)
            {
                _context.Car.Remove(car); // Удаляем автомобиль из контекста
                _context.SaveChanges(); // Сохраняем изменения в базе данных
            }

            return await VerifiedObjects();
        }

        [HttpPost]
        public async Task<IActionResult> DeleteAttractionVerified(int deleteAttractionId)
        {
            var attraction = _context.Entertainment.Find(deleteAttractionId);
            var objectsToDelete = _context.ObjectPhotos.Where(o => o.ObjectId == deleteAttractionId).ToList();

            if (objectsToDelete.Any())
            {
                // Удаляем найденные объекты
                _context.ObjectPhotos.RemoveRange(objectsToDelete);
                await _context.SaveChangesAsync();
            }

            if (attraction != null)
            {
                _context.Entertainment.Remove(attraction); 
                _context.SaveChanges(); // Сохраняем изменения в базе данных
            }

            return await VerifiedObjects();
        }

        [HttpPost]
        public async Task<IActionResult> DeleteAccomodationVerified(int deleteAccomodationId)
        {
            var accomodation = _context.Accomodation.Find(deleteAccomodationId);
            var objectsToDelete = _context.AccomodationPhotos.Where(o => o.ObjectId == deleteAccomodationId).ToList();

            if (objectsToDelete.Any())
            {
                // Удаляем найденные объекты
                _context.AccomodationPhotos.RemoveRange(objectsToDelete);
                await _context.SaveChangesAsync();
            }

            if (accomodation != null)
            {
                _context.Accomodation.Remove(accomodation);
                _context.SaveChanges(); // Сохраняем изменения в базе данных
            }

            return await VerifiedObjects();
        }

       [HttpPost]
        public async Task<IActionResult> DeleteCarRejected(int deleteCarId)
        {
            var car = _context.Car.Find(deleteCarId); // Находим автомобиль по его ID

            if (car != null)
            {
                _context.Car.Remove(car); // Удаляем автомобиль из контекста
                _context.SaveChanges(); // Сохраняем изменения в базе данных
            }

            return await RejectedObjects();
        }

        [HttpPost]
        public async Task<IActionResult> DeleteAttractionRejected(int deleteAttractionId)
        {
            var attraction = _context.Entertainment.Find(deleteAttractionId);
            var objectsToDelete = _context.ObjectPhotos.Where(o => o.ObjectId == deleteAttractionId).ToList();

            if (objectsToDelete.Any())
            {
                // Удаляем найденные объекты
                _context.ObjectPhotos.RemoveRange(objectsToDelete);
                await _context.SaveChangesAsync();
            }

            if (attraction != null)
            {
                _context.Entertainment.Remove(attraction); // Удаляем автомобиль из контекста
                _context.SaveChanges(); // Сохраняем изменения в базе данных
            }

            return await RejectedObjects();
        }

        [HttpPost]
        public async Task<IActionResult> DeleteAccomodationRejected(int deleteAccomodationId)
        {
            var accomodation = _context.Accomodation.Find(deleteAccomodationId);
            var objectsToDelete = _context.AccomodationPhotos.Where(o => o.ObjectId == deleteAccomodationId).ToList();

            if (objectsToDelete.Any())
            {
                // Удаляем найденные объекты
                _context.AccomodationPhotos.RemoveRange(objectsToDelete);
                await _context.SaveChangesAsync();
            }

            if (accomodation != null)
            {
                _context.Accomodation.Remove(accomodation); // Удаляем автомобиль из контекста
                _context.SaveChanges(); // Сохраняем изменения в базе данных
            }

            return await RejectedObjects();
        }

       [HttpPost]
        public async Task<IActionResult> DeleteAirTicketECUnverified(int deleteAirTicketECId)
        {
            var airticket = _context.AirTicket.Find(deleteAirTicketECId); // Находим автомобиль по его ID
            airticket.AmountOfTicketsEC = 0;

            if (airticket.AmountOfTicketsEC == 0 && airticket.AmountOfTicketsBC == 0 && airticket.AmountOfTicketsFC == 0)
            {
                _context.AirTicket.Remove(airticket); // Удаляем автомобиль из контекста
            }
            await _context.SaveChangesAsync(); // Сохраняем изменения в базе данных
            return await UnverifiedObjects();
        }

        [HttpPost]
        public async Task<IActionResult> DeleteAirTicketBCUnverified(int deleteAirTicketBCId)
        {
            var airticket = _context.AirTicket.Find(deleteAirTicketBCId); // Находим автомобиль по его ID
            airticket.AmountOfTicketsBC = 0;

            if (airticket.AmountOfTicketsEC == 0 && airticket.AmountOfTicketsBC == 0 && airticket.AmountOfTicketsFC == 0)
            {
                _context.AirTicket.Remove(airticket); // Удаляем автомобиль из контекста
            }
            await _context.SaveChangesAsync();// Сохраняем изменения в базе данных
            return await UnverifiedObjects();
        }

        [HttpPost]
        public async Task<IActionResult> DeleteAirTicketFCUnverified(int deleteAirTicketFCId)
        {
            var airticket = _context.AirTicket.Find(deleteAirTicketFCId); // Находим автомобиль по его ID
            airticket.AmountOfTicketsFC = 0;

            if (airticket.AmountOfTicketsEC == 0 && airticket.AmountOfTicketsBC == 0 && airticket.AmountOfTicketsFC == 0)
            {
                _context.AirTicket.Remove(airticket); // Удаляем автомобиль из контекста
            }
            await _context.SaveChangesAsync(); // Сохраняем изменения в базе данных
            return await UnverifiedObjects();
        }

        [HttpPost]
        public async Task<IActionResult> DeleteAirTicketECVerified(int deleteAirTicketECId)
        {
            var airticket = _context.AirTicket.Find(deleteAirTicketECId); // Находим автомобиль по его ID
            airticket.AmountOfTicketsEC = 0;

            if (airticket.AmountOfTicketsEC == 0 && airticket.AmountOfTicketsBC == 0 && airticket.AmountOfTicketsFC == 0)
            {
                _context.AirTicket.Remove(airticket); // Удаляем автомобиль из контекста
            }
            await _context.SaveChangesAsync();// Сохраняем изменения в базе данных
            return await VerifiedObjects();
        }

        [HttpPost]
        public async Task<IActionResult> DeleteAirTicketBCVerified(int deleteAirTicketBCId)
        {
            var airticket = _context.AirTicket.Find(deleteAirTicketBCId); // Находим автомобиль по его ID
            airticket.AmountOfTicketsBC = 0;

            if (airticket.AmountOfTicketsEC == 0 && airticket.AmountOfTicketsBC == 0 && airticket.AmountOfTicketsFC == 0)
            {
                _context.AirTicket.Remove(airticket); // Удаляем автомобиль из контекста
            }
            await _context.SaveChangesAsync(); // Сохраняем изменения в базе данных
            return await VerifiedObjects();
        }

        [HttpPost]
        public async Task<IActionResult> DeleteAirTicketFCVerified(int deleteAirTicketFCId)
        {
            var airticket = _context.AirTicket.Find(deleteAirTicketFCId); // Находим автомобиль по его ID
            airticket.AmountOfTicketsFC = 0;

            if (airticket.AmountOfTicketsEC == 0 && airticket.AmountOfTicketsBC == 0 && airticket.AmountOfTicketsFC == 0)
            {
                _context.AirTicket.Remove(airticket); // Удаляем автомобиль из контекста
            }
            await _context.SaveChangesAsync();// Сохраняем изменения в базе данных
            return await VerifiedObjects();
        }

        [HttpPost]
        public async Task<IActionResult> DeleteAirTicketECRejected(int deleteAirTicketECId)
        {
            var airticket = _context.AirTicket.Find(deleteAirTicketECId); // Находим автомобиль по его ID
            airticket.AmountOfTicketsEC = 0;

            if (airticket.AmountOfTicketsEC == 0 && airticket.AmountOfTicketsBC == 0 && airticket.AmountOfTicketsFC == 0)
            {
                _context.AirTicket.Remove(airticket); // Удаляем автомобиль из контекста
            }
            await _context.SaveChangesAsync(); // Сохраняем изменения в базе данных
            return await RejectedObjects();
        }

        [HttpPost]
        public async Task<IActionResult> DeleteAirTicketBCRejected(int deleteAirTicketBCId)
        {
            var airticket = _context.AirTicket.Find(deleteAirTicketBCId); // Находим автомобиль по его ID
            airticket.AmountOfTicketsBC = 0;

            if (airticket.AmountOfTicketsEC == 0 && airticket.AmountOfTicketsBC == 0 && airticket.AmountOfTicketsFC == 0)
            {
                _context.AirTicket.Remove(airticket); // Удаляем автомобиль из контекста
            }
            await _context.SaveChangesAsync(); // Сохраняем изменения в базе данных
            return await RejectedObjects();
        }

        [HttpPost]
        public async Task<IActionResult> DeleteAirTicketFCRejected(int deleteAirTicketFCId)
        {
            var airticket = _context.AirTicket.Find(deleteAirTicketFCId); // Находим автомобиль по его ID
            airticket.AmountOfTicketsFC = 0;

            if (airticket.AmountOfTicketsEC == 0 && airticket.AmountOfTicketsBC == 0 && airticket.AmountOfTicketsFC == 0)
            {
                _context.AirTicket.Remove(airticket); // Удаляем автомобиль из контекста
            }
            await _context.SaveChangesAsync(); // Сохраняем изменения в базе данных
            return await RejectedObjects();
        }

        [HttpPost]
        public async Task<IActionResult> EditCarUnverified(int editCarId)
        {
            var car = _context.Car.Find(editCarId); // Находим автомобиль по его ID
            ViewBag.CarPhoto = car.CarPhoto;
            ViewBag.CarModel = car.Title;
            ViewBag.CarCountry = car.Country;
            ViewBag.City = car.City;
            ViewBag.CarAddress = car.Address;
            ViewBag.IsAirCondition = car.IsAirCondition;
            ViewBag.CarTransmission = car.Transmission;
            ViewBag.UnlimitedMileage = car.UnlimitedMileage;
            ViewBag.ElectricCar = car.ElectricCar;
            ViewBag.CarCategory = car.CarCategory;
            ViewBag.CarPassengerCapacity = car.AmountOfPassengers;
            ViewBag.TheftCoverage = car.TheftCoverage;
            ViewBag.CollisionDamage = car.CollisionDamageWaiver;
            ViewBag.LiabilityCoverage = car.LiabilityCoverage;
            ViewBag.FreeCancellation = car.FreeCancellation;
            ViewBag.CarDescription = car.Description;
            ViewBag.CarPrice = car.Cost;

            List<DateTime> DatesStart = car.StartDates;
            List<DateTime> DatesEnd = car.EndDates;

            ViewBag.DatesStart = DatesStart;
            ViewBag.DatesEnd = DatesEnd;

            ViewBag.editCarId = car.Id;

            return View("EditCarPage");
        }

        [HttpPost]
        public async Task<IActionResult> EditAttractionUnverified(int editAttractionId)
        {
            var attraction = _context.Entertainment.Find(editAttractionId);
            ViewBag.AttractionPhotos = _context.ObjectPhotos
                                       .Where(photo => photo.ObjectId == editAttractionId)
                                       .Select(photo => photo.PhotoArray)
                                       .ToList(); ;
            ViewBag.AttractionName = attraction.Title;
            ViewBag.AttractionCountry = attraction.Country;
            ViewBag.AttractionCity = attraction.City;
            ViewBag.AttractionAddress = attraction.Address;
            ViewBag.Languages = attraction.Languages;
            ViewBag.AttractionCategory = attraction.Category;
            ViewBag.TimeOfDay = attraction.TimeOfDay;
            ViewBag.FreeCancellation = attraction.FreeCancellation;
            ViewBag.AttractionDescription = attraction.Description;
            ViewBag.AttractionPrice = attraction.Cost;

            List<DateTime> Dates = attraction.AvailableDates;
            List<int> AmountOfTickets = attraction.AmountOfTickets;

            ViewBag.Dates = Dates;
            ViewBag.AmountOfTickets = AmountOfTickets;

            ViewBag.editAttractionId = attraction.Id;

            return View("EditAttractionPage");
        }

        [HttpPost]
        public async Task<IActionResult> EditAccomodationUnverified(int editAccomodationId)
        {
            var accomodation = _context.Accomodation.Find(editAccomodationId);
            ViewBag.AccomodationPhotos = _context.AccomodationPhotos
                                       .Where(photo => photo.ObjectId == editAccomodationId)
                                       .Select(photo => photo.PhotoArray)
                                       .ToList(); ;
            ViewBag.AccomodationName = accomodation.Name;
            ViewBag.AccomodationCountry = accomodation.Country;
            ViewBag.AccomodationCity = accomodation.City;
            ViewBag.AccomodationAddress = accomodation.Address;
            ViewBag.AccomodationType = accomodation.TypeOfAccomodation;
            ViewBag.Nutrition = accomodation.TypesOfNutrition;
            ViewBag.Parking = accomodation.Parking;
            ViewBag.SwimmingPool = accomodation.SwimmingPool;
            ViewBag.FreeWIFI = accomodation.FreeWIFI;
            ViewBag.PrivateBeach = accomodation.PrivateBeach;
            ViewBag.LineOfBeach = accomodation.LineOfBeach.ToString();
            ViewBag.SPA = accomodation.SPA;
            ViewBag.Bar = accomodation.Bar;
            ViewBag.Restaurants = accomodation.Restaurants;
            ViewBag.Garden = accomodation.Garden;
            ViewBag.TransferToAirport = accomodation.TransferToAirport;
            ViewBag.TactileSigns = accomodation.TactileSigns;
            ViewBag.SmookingRooms = accomodation.SmookingRooms;
            ViewBag.FamilyRooms = accomodation.FamilyRooms;
            ViewBag.CarChargingStation = accomodation.CarChargingStation;
            ViewBag.WheelchairAccessible = accomodation.WheelchairAccessible;
            ViewBag.FitnessCentre = accomodation.FitnessCentre;
            ViewBag.PetsAllowed = accomodation.PetsAllowed;
            ViewBag.DeliveryFoodToTheRoom = accomodation.DeliveryFoodToTheRoom;
            ViewBag.EveryHourFrontDesk = accomodation.EveryHourFrontDesk;
            ViewBag.Description = accomodation.Description;


            ViewBag.editAccomodationId = accomodation.Id;

            return View("EditAccomodationPage");
        }

        [HttpPost]
        public async Task<IActionResult> EditAirTicketECUnverified(int editAirticketId)
        {
            var airticket = _context.AirTicket.Find(editAirticketId);
            ViewBag.DepartureCountry = airticket.CountryFrom;
            ViewBag.DepartureCityCode = airticket.DepartureCountryCode;
            ViewBag.DepartureCity = airticket.CityFrom;
            ViewBag.ArrivalCountry = airticket.CountryTo;
            ViewBag.ArrivalCityCode = airticket.ArrivalCountryCode;
            ViewBag.ArrivalCity = airticket.CityTo;
            ViewBag.FlightNumber = airticket.FlightNumber;
            ViewBag.FreeCancellation = airticket.FreeCancellation;
            ViewBag.AmountOfTicketsEC = airticket.AmountOfTicketsEC;
            ViewBag.CostEC = airticket.CostEC;
            ViewBag.IncludeLuggageEC = airticket.IncludeLuggageEC;
            ViewBag.AmountOfTicketsBC = airticket.AmountOfTicketsBC;
            ViewBag.CostBC = airticket.CostBC;
            ViewBag.IncludeLuggageBC = airticket.IncludeLuggageBC;
            ViewBag.AmountOfTicketsFC = airticket.AmountOfTicketsFC;
            ViewBag.CostFC = airticket.CostFC;
            ViewBag.IncludeLuggageFC = airticket.IncludeLuggageFC;

            ViewBag.DepartureTime = airticket.DepartureTime;
            ViewBag.ArrivalTime = airticket.ArrivalTime;

            ViewBag.editAirticketId = airticket.Id;

            return View("EditAirTicketPage");
        }

        [HttpPost]
        public async Task<IActionResult> UpdateForm(int editCarId, [Bind("Title, City, Country, Address, IsAirCondition, Transmission, Cost, FreeCancellation, AmountOfPassengers, TheftCoverage, CollisionDamageWaiver, LiabilityCoverage, UnlimitedMileage, CarCategory, ElectricCar, Description, StartDates, EndDates", Prefix = "model")] Car model)
        {
            var currentUser = await _context.Users.FirstOrDefaultAsync(u => u.UserName == User.Identity.Name);

            var car = _context.Car.Find(editCarId);

            string filePath = "C:\\Users\\Angelina\\Pictures\\Camera Roll\\bbb846030d108b92a3fefbfca1f7bbe6.jpg";
            byte[] fileBytes = System.IO.File.ReadAllBytes(filePath);

            var file = Request.Form.Files["file"]; // Получаем файл из запроса
            byte[] imageDataa = fileBytes;

            if (file != null && file.Length > 0)
            {
                using (var memoryStream = new MemoryStream())
                {
                    await file.CopyToAsync(memoryStream);
                    byte[] imageData = memoryStream.ToArray();
                    ViewBag.CarPhoto = imageData;
                    imageDataa = imageData;
                }
            }
            else {
                ViewBag.CarPhoto = car.CarPhoto;
                imageDataa = car.CarPhoto;
            }

            car.Title = model.Title;
            car.City = model.City;
            car.Country = model.Country;
            car.Address = model.Address;
            car.IsAirCondition = model.IsAirCondition;
            car.Transmission = model.Transmission;
            car.Cost = model.Cost;
            car.CarPhoto = imageDataa;
            car.FreeCancellation = model.FreeCancellation;
            car.AmountOfPassengers = model.AmountOfPassengers;
            car.TheftCoverage = model.TheftCoverage;
            car.CollisionDamageWaiver = model.CollisionDamageWaiver;
            car.LiabilityCoverage = model.LiabilityCoverage;
            car.UnlimitedMileage = model.UnlimitedMileage;
            car.CarCategory = model.CarCategory;
            car.ElectricCar = model.ElectricCar;
            car.Description = model.Description;
            car.StartDates = model.StartDates;
            car.EndDates = model.EndDates;
            car.PublisherId = currentUser.Id;
            car.VerifiedByAdmin = false;
            car.RejectedByAdmin = false;

            await _context.SaveChangesAsync(); // Сохраняем изменения в базе данных
            return await UnverifiedObjects();

        }

        [HttpPost]
        public async Task<IActionResult> UpdateFlightForm(int editAirticketId, [Bind("CountryFrom, DepartureCountryCode, CityFrom, CountryTo, ArrivalCountryCode, CityTo, FlightNumber, FreeCancellation, DepartureTime, ArrivalTime, AmountOfTicketsEC, AmountOfTicketsBC, AmountOfTicketsFC, CostEC, CostBC, CostFC, IncludeLuggageEC, IncludeLuggageBC, IncludeLuggageFC", Prefix = "model")] AirTicket model)
        {
            var currentUser = await _context.Users.FirstOrDefaultAsync(u => u.UserName == User.Identity.Name);

            var airticket = _context.AirTicket.Find(editAirticketId);

            string filePath = "C:\\Users\\Angelina\\Pictures\\Camera Roll\\bbb846030d108b92a3fefbfca1f7bbe6.jpg";
            byte[] fileBytes = System.IO.File.ReadAllBytes(filePath);
            ViewBag.CarPhoto = fileBytes;

            airticket.CountryFrom = model.CountryFrom;
            airticket.DepartureCountryCode = model.DepartureCountryCode;
            airticket.CityFrom = model.CityFrom;
            airticket.CountryTo = model.CountryTo;
            airticket.ArrivalCountryCode = model.ArrivalCountryCode;
            airticket.CityTo = model.CityTo;
            airticket.FlightNumber = model.FlightNumber;
            airticket.DepartureTime = model.DepartureTime;
            airticket.ArrivalTime = model.ArrivalTime;
            airticket.AmountOfTicketsEC = model.AmountOfTicketsEC;
            airticket.AmountOfTicketsBC = model.AmountOfTicketsBC;
            airticket.AmountOfTicketsFC = model.AmountOfTicketsFC;
            airticket.CostEC = model.CostEC;
            airticket.CostBC = model.CostBC;
            airticket.CostFC = model.CostFC;
            airticket.IncludeLuggageEC = model.IncludeLuggageEC;
            airticket.IncludeLuggageBC = model.IncludeLuggageBC;
            airticket.IncludeLuggageFC = model.IncludeLuggageFC;
            airticket.PublisherId = currentUser.Id;
            airticket.VerifiedByAdminECTicket = false;
            airticket.VerifiedByAdminBCTicket = false;
            airticket.VerifiedByAdminFCTicket = false;
            airticket.RejectedByAdminEC = false;
            airticket.RejectedByAdminBC = false;
            airticket.RejectedByAdminFC = false;

            await _context.SaveChangesAsync(); // Сохраняем изменения в базе данных
            return await UnverifiedObjects();
        }

        [HttpPost]
        public async Task<IActionResult> UpdateEntertainmentForm(int editAttractionId, Entertainment model, string photoData)
        {
            var currentUser = await _context.Users.FirstOrDefaultAsync(u => u.UserName == User.Identity.Name);

            var attraction = _context.Entertainment.Find(editAttractionId);

            string filePath = "C:\\Users\\Angelina\\Pictures\\Camera Roll\\bbb846030d108b92a3fefbfca1f7bbe6.jpg";
            byte[] fileBytes = System.IO.File.ReadAllBytes(filePath);
            ViewBag.CarPhoto = fileBytes;

            var photos = JsonConvert.DeserializeObject<List<string>>(photoData);
            var photoBytesList = photos.Select(photoBase64 => Convert.FromBase64String(photoBase64.Split(',')[1])).ToList();

            attraction.Title = model.Title;
            attraction.City = model.City;
            attraction.Country = model.Country;
            attraction.Address = model.Address;
            attraction.Languages = model.Languages;
            attraction.TimeOfDay = model.TimeOfDay;
            attraction.Cost = model.Cost;
            attraction.FreeCancellation = model.FreeCancellation;
            attraction.Category = model.Category;
            attraction.MainPhoto = photoBytesList[0];
            attraction.Description = model.Description;
            attraction.AvailableDates = model.AvailableDates;
            attraction.AmountOfTickets = model.AmountOfTickets;
            attraction.VerifiedByAdmin = false;
            attraction.PublisherId = currentUser.Id;
            attraction.RejectedMessage = string.Empty;
            attraction.RejectedByAdmin = false;

            var photosToDelete = _context.ObjectPhotos.Where(photo => photo.ObjectId == editAttractionId).ToList();

            if (photosToDelete.Any())
            {
                // Удаляем найденные фотографии
                _context.ObjectPhotos.RemoveRange(photosToDelete);

                // Сохраняем изменения в базе данных
                _context.SaveChanges();
            }

            foreach (var photo in photoBytesList)
            {
                var photoss = new Photos
                {
                    ObjectId = editAttractionId,
                    PhotoArray = photo,
                };
                _context.ObjectPhotos.Add(photoss);
                await _context.SaveChangesAsync();
            }

            await _context.SaveChangesAsync(); // Сохраняем изменения в базе данных
            return await UnverifiedObjects();
        }

        [HttpPost]
        public async Task<IActionResult> UpdateAccomodationForm(int editAccomodationId, Accomodation model, string photoDataAccomodation)
        {
            var currentUser = await _context.Users.FirstOrDefaultAsync(u => u.UserName == User.Identity.Name);

            var accomodation = _context.Accomodation.Find(editAccomodationId);

            string filePath = "C:\\Users\\Angelina\\Pictures\\Camera Roll\\bbb846030d108b92a3fefbfca1f7bbe6.jpg";
            byte[] fileBytes = System.IO.File.ReadAllBytes(filePath);
            ViewBag.CarPhoto = fileBytes;

            var photos = JsonConvert.DeserializeObject<List<string>>(photoDataAccomodation);
            var photoBytesList = photos.Select(photoBase64 => Convert.FromBase64String(photoBase64.Split(',')[1])).ToList();

            accomodation.Name = model.Name;
            accomodation.City = model.City;
            accomodation.Country = model.Country;
            accomodation.Address = model.Address;
            accomodation.MainPhoto = photoBytesList[0];
            accomodation.Description = model.Description;
            accomodation.VerifiedByAdmin = false;
            accomodation.PublisherId = currentUser.Id;
            accomodation.RejectedMessage = string.Empty;
            accomodation.RejectedByAdmin = false;
            accomodation.TypeOfAccomodation = model.TypeOfAccomodation;
            accomodation.TypesOfNutrition = model.TypesOfNutrition;
            accomodation.Parking = model.Parking;
            accomodation.SwimmingPool = model.SwimmingPool;
            accomodation.FreeWIFI = model.FreeWIFI;
            accomodation.PrivateBeach = model.PrivateBeach;
            accomodation.LineOfBeach = model.LineOfBeach;
            accomodation.SPA = model.SPA;
            accomodation.Bar = model.Bar;
            accomodation.Restaurants = model.Restaurants;
            accomodation.Garden = model.Garden;
            accomodation.TransferToAirport = model.TransferToAirport;
            accomodation.TactileSigns = model.TactileSigns;
            accomodation.SmookingRooms = model.SmookingRooms;
            accomodation.FamilyRooms = model.FamilyRooms;
            accomodation.CarChargingStation = model.CarChargingStation;
            accomodation.WheelchairAccessible = model.WheelchairAccessible;
            accomodation.FitnessCentre = model.FitnessCentre;
            accomodation.PetsAllowed = model.PetsAllowed;
            accomodation.DeliveryFoodToTheRoom = model.DeliveryFoodToTheRoom;
            accomodation.EveryHourFrontDesk = model.EveryHourFrontDesk;

            var photosToDelete = _context.AccomodationPhotos.Where(photo => photo.ObjectId == editAccomodationId).ToList();

            if (photosToDelete.Any())
            {
                // Удаляем найденные фотографии
                _context.AccomodationPhotos.RemoveRange(photosToDelete);

                // Сохраняем изменения в базе данных
                _context.SaveChanges();
            }

            foreach (var photo in photoBytesList)
            {
                var photoss = new AccomodationPhotos
                {
                    ObjectId = editAccomodationId,
                    PhotoArray = photo,
                };
                _context.AccomodationPhotos.Add(photoss);
                await _context.SaveChangesAsync();
            }

            await _context.SaveChangesAsync(); // Сохраняем изменения в базе данных
            return await UnverifiedObjects();
        }

       [HttpPost]
        public async Task<IActionResult> DeleteCarPhotoEdit(int editCarId)
        {
            var car = _context.Car.Find(editCarId);

            string filePath = "C:\\Users\\Angelina\\Pictures\\Camera Roll\\bbb846030d108b92a3fefbfca1f7bbe6.jpg";
            byte[] fileBytes = System.IO.File.ReadAllBytes(filePath);
            ViewBag.CarPhoto = fileBytes;

            car.CarPhoto = fileBytes;
            await _context.SaveChangesAsync();

            return await EditCarUnverified(editCarId);
        }


        public async Task<IActionResult> ShowCarInformation(int carId)
        {
            var car = _context.Car.Find(carId);

            string filePath = "C:\\Users\\Angelina\\Pictures\\Camera Roll\\bbb846030d108b92a3fefbfca1f7bbe6.jpg";
            byte[] fileBytes = System.IO.File.ReadAllBytes(filePath);
            ViewBag.CarPhoto = fileBytes;

            var model = new ShowCarInformationViewModel
            {
                Title = car.Title,
                City = car.City,
                Country = car.Country,
                IsAirCondition = car.IsAirCondition,
                Transmission = car.Transmission,
                Address = car.Address,
                Cost = car.Cost,
                CarPhoto = car.CarPhoto,
                FreeCancellation = car.FreeCancellation,
                AmountOfPassengers = car.AmountOfPassengers,
                TheftCoverage = car.TheftCoverage,
                CollisionDamageWaiver = car.CollisionDamageWaiver,
                LiabilityCoverage = car.LiabilityCoverage,
                UnlimitedMileage = car.UnlimitedMileage,
                CarCategory = car.CarCategory,
                ElectricCar = car.ElectricCar,
                Description = car.Description,
                StartDates = car.StartDates,
                EndDates = car.EndDates
            };

            return View("CarInformation", model);
        }

        public async Task<IActionResult> ShowAttractionInformation(int attractionId)
        {
            var attraction = _context.Entertainment.Find(attractionId);

            string filePath = "C:\\Users\\Angelina\\Pictures\\Camera Roll\\bbb846030d108b92a3fefbfca1f7bbe6.jpg";
            byte[] fileBytes = System.IO.File.ReadAllBytes(filePath);
            ViewBag.CarPhoto = fileBytes;

            var photos = _context.ObjectPhotos
                        .Where(photo => photo.ObjectId == attractionId) 
                        .Select(photo => photo.PhotoArray)
                        .ToList();

            var model = new ShowAttractionInformationViewModel
            {
                Title = attraction.Title,
                City = attraction.City,
                Country = attraction.Country,
                Address = attraction.Address,
                AvailableDates = attraction.AvailableDates,
                Description = attraction.Description,
                Cost = attraction.Cost,
                Category = attraction.Category,
                FreeCancellation = attraction.FreeCancellation,
                //MainPhoto { get; set; }
                Photos = photos,
                AmountOfTickets = attraction.AmountOfTickets,
                Languages = attraction.Languages,
                TimeOfDay = attraction.TimeOfDay
            };

            return View("AttractionInformation", model);
        }

        public async Task<IActionResult> ShowAccomodationInformation(int accomodationId)
        {
            var accomodation = _context.Accomodation.Find(accomodationId);

            string filePath = "C:\\Users\\Angelina\\Pictures\\Camera Roll\\bbb846030d108b92a3fefbfca1f7bbe6.jpg";
            byte[] fileBytes = System.IO.File.ReadAllBytes(filePath);
            ViewBag.CarPhoto = fileBytes;

            var photos = _context.AccomodationPhotos
                        .Where(photo => photo.ObjectId == accomodationId)
                        .Select(photo => photo.PhotoArray)
                        .ToList();
            var roomphotos = _context.RoomPhotos
                        .Where(photo => photo.ObjectId == accomodationId)
                        .Select(photo => photo.PhotoArray)
                        .ToList();
            var rooms = _context.Rooms
                        .Where(room => room.AccomodationId == accomodationId)
                        .ToList();
            
            foreach(var room in rooms)
            {
                room.Photos = _context.RoomPhotos
                        .Where(photo => photo.ObjectId == accomodationId && photo.RoomId == room.ID)
                        .ToList();
            }

            var model = new ShowAccomodationInformationViewModel
            {
                Name = accomodation.Name,
                City = accomodation.City,
                Country = accomodation.Country,
                Address = accomodation.Address,
                TypeOfAccomodation = accomodation.TypeOfAccomodation,
                TypesOfNutrition = accomodation.TypesOfNutrition,
                Parking = accomodation.Parking,
                SwimmingPool = accomodation.SwimmingPool,
                FreeWIFI = accomodation.FreeWIFI,
                Photos = photos,
                PrivateBeach = accomodation.PrivateBeach,
                LineOfBeach = accomodation.LineOfBeach,
                Restaurants = accomodation.Restaurants,
                SPA = accomodation.SPA,
                Bar = accomodation.Bar,
                Garden = accomodation.Garden,
                AllRooms = rooms,
                TransferToAirport = accomodation.TransferToAirport,
                SmookingRooms = accomodation.SmookingRooms,
                FamilyRooms = accomodation.FamilyRooms,
                CarChargingStation = accomodation.CarChargingStation,
                WheelchairAccessible = accomodation.WheelchairAccessible,
                FitnessCentre = accomodation.FitnessCentre,
                PetsAllowed = accomodation.PetsAllowed,
                DeliveryFoodToTheRoom = accomodation.DeliveryFoodToTheRoom,
                EveryHourFrontDesk = accomodation.EveryHourFrontDesk,
                Description = accomodation.Description
            };

            return View("AccomodationInformation", model);
        }

        public async Task<IActionResult> AllVerifiedObjects()
        {

            var verifiedCars = await _context.Car
            .Where(c => c.VerifiedByAdmin)
            .ToListAsync();

            var verifiedAirTickets = await _context.AirTicket
           .Where(c => c.VerifiedByAdminECTicket || c.VerifiedByAdminBCTicket || c.VerifiedByAdminFCTicket)
           .ToListAsync();

            var verifiedAttractions = await _context.Entertainment
            .Where(c => c.VerifiedByAdmin)
            .ToListAsync();

            var verifiedAccomodation = await _context.Accomodation
           .Where(c => c.VerifiedByAdmin)
           .ToListAsync();

            var model = new VerifiedObjectsViewModel
            {
                VerifiedCars = verifiedCars,
                VerifiedAirTickets = verifiedAirTickets,
                VerifiedAttractions = verifiedAttractions,
                VerifiedAccomodation = verifiedAccomodation
            };

            return View("VerifiedItems", model);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteCar(int deleteCarId)
        {
            var car = _context.Car.Find(deleteCarId); // Находим автомобиль по его ID

            if (car != null)
            {
                _context.Car.Remove(car); // Удаляем автомобиль из контекста
                _context.SaveChanges(); // Сохраняем изменения в базе данных
            }

            return await AllVerifiedObjects();
        }

        [HttpPost]
        public async Task<IActionResult> DeleteAttraction(int deleteAttractionId)
        {
            var attraction = _context.Entertainment.Find(deleteAttractionId);
            var objectsToDelete = _context.ObjectPhotos.Where(o => o.ObjectId == deleteAttractionId).ToList();

            if (objectsToDelete.Any())
            {
                // Удаляем найденные объекты
                _context.ObjectPhotos.RemoveRange(objectsToDelete);
                await _context.SaveChangesAsync();
            }


            if (attraction != null)
            {
                _context.Entertainment.Remove(attraction);
                _context.SaveChanges(); // Сохраняем изменения в базе данных
            }

            return await AllVerifiedObjects();
        }

       [HttpPost]
        public async Task<IActionResult> DeleteAllAirTicketECVerified(int deleteAirTicketECId)
        {
            var airticket = _context.AirTicket.Find(deleteAirTicketECId); // Находим автомобиль по его ID
            airticket.AmountOfTicketsEC = 0;

            if (airticket.AmountOfTicketsEC == 0 && airticket.AmountOfTicketsBC == 0 && airticket.AmountOfTicketsFC == 0)
            {
                _context.AirTicket.Remove(airticket); // Удаляем автомобиль из контекста
            }
            await _context.SaveChangesAsync();// Сохраняем изменения в базе данных
            return await AllVerifiedObjects();
        }

        [HttpPost]
        public async Task<IActionResult> DeleteAllAirTicketBCVerified(int deleteAirTicketBCId)
        {
            var airticket = _context.AirTicket.Find(deleteAirTicketBCId); // Находим автомобиль по его ID
            airticket.AmountOfTicketsBC = 0;

            if (airticket.AmountOfTicketsEC == 0 && airticket.AmountOfTicketsBC == 0 && airticket.AmountOfTicketsFC == 0)
            {
                _context.AirTicket.Remove(airticket); // Удаляем автомобиль из контекста
            }
            await _context.SaveChangesAsync(); // Сохраняем изменения в базе данных
            return await AllVerifiedObjects();
        }

        [HttpPost]
        public async Task<IActionResult> DeleteAllAirTicketFCVerified(int deleteAirTicketFCId)
        {
            var airticket = _context.AirTicket.Find(deleteAirTicketFCId); // Находим автомобиль по его ID
            airticket.AmountOfTicketsFC = 0;

            if (airticket.AmountOfTicketsEC == 0 && airticket.AmountOfTicketsBC == 0 && airticket.AmountOfTicketsFC == 0)
            {
                _context.AirTicket.Remove(airticket); // Удаляем автомобиль из контекста
            }
            await _context.SaveChangesAsync();// Сохраняем изменения в базе данных
            return await AllVerifiedObjects();
        }

        public async Task<IActionResult> AllUnverifiedObjects()
        {
            var unverifiedCars = await _context.Car
            .Where(c => !c.VerifiedByAdmin && !c.RejectedByAdmin)
            .ToListAsync();

            var unverifiedAirTickets = await _context.AirTicket
           .Where(c => ((!c.VerifiedByAdminECTicket && !c.RejectedByAdminEC) || (!c.VerifiedByAdminBCTicket && !c.RejectedByAdminBC) || (!c.VerifiedByAdminFCTicket && !c.RejectedByAdminFC)))
           .ToListAsync();

            var unverifiedAttractions = await _context.Entertainment
           .Where(c => !c.VerifiedByAdmin && !c.RejectedByAdmin)
           .ToListAsync();

            var unverifiedAccomodation = await _context.Accomodation
           .Where(c => !c.VerifiedByAdmin && !c.RejectedByAdmin)
           .ToListAsync();

            var model = new UnverifiedObjectsViewModel
            {
                UnverifiedCars = unverifiedCars,
                UnverifiedAirTickets = unverifiedAirTickets,
                UnverifiedAttractions = unverifiedAttractions,
                UnverifiedAccomodation = unverifiedAccomodation
            };

            return View("UnverifiedItems", model);
        }

        public async Task<IActionResult> VerifyCar(int verifyCarId)
        {
            var car = _context.Car.Find(verifyCarId);

            car.VerifiedByAdmin = true;

            await _context.SaveChangesAsync();

            return await AllUnverifiedObjects();
        }

        public async Task<IActionResult> VerifyCarBooking(int verifyCarId)
        {
            var car = _context.BookingCars.Find(verifyCarId);

            car.VerifiedBooking = true;
            car.RejectedMessage = string.Empty;
            car.RejectedBooking = false;
            car.CanceledBooking = false;

            await _context.SaveChangesAsync();

            return await BookingsForVerification();
        }

        public async Task<IActionResult> VerifyAttraction(int verifyAttractionId)
        {
            var attraction = _context.Entertainment.Find(verifyAttractionId);

            attraction.VerifiedByAdmin = true;

            await _context.SaveChangesAsync();

            return await AllUnverifiedObjects();
        }

        public async Task<IActionResult> VerifyAttractionBooking(int verifyAttractionId)
        {
            var attraction = _context.BookingAttractions.Find(verifyAttractionId);

            attraction.VerifiedBooking = true;
            attraction.RejectedMessage = string.Empty;
            attraction.RejectedBooking = false;
            attraction.CanceledBooking = false;

            await _context.SaveChangesAsync();

            return await BookingsForVerification();
        }

        public async Task<IActionResult> VerifyAccomodation(int verifyAccomodationId)
        {
            var accomodation = _context.Accomodation.Find(verifyAccomodationId);

            accomodation.VerifiedByAdmin = true;

            await _context.SaveChangesAsync();

            return await AllUnverifiedObjects();
        }

        public async Task<IActionResult> VerifyAccomodationBooking(int verifyAccomodationId)
        {
            var accomodation = _context.BookingAccomodations.Find(verifyAccomodationId);

            accomodation.VerifiedBooking = true;
            accomodation.RejectedMessage = string.Empty;
            accomodation.RejectedBooking = false;
            accomodation.CanceledBooking = false;

            _context.BookingAccomodations.Update(accomodation);
            await _context.SaveChangesAsync();

            return await BookingsForVerification();
        }

        public async Task<IActionResult> VerifyRejectedCar(int verifyCarId)
        {
            var car = _context.Car.Find(verifyCarId);

            car.VerifiedByAdmin = true;
            car.RejectedByAdmin = false;
            car.RejectedMessage = string.Empty;

            await _context.SaveChangesAsync();

            return await AllRejectedObjects();
        }

        public async Task<IActionResult> VerifyRejectedAttraction(int verifyAttractionId)
        {
            var attraction = _context.Entertainment.Find(verifyAttractionId);

            attraction.VerifiedByAdmin = true;
            attraction.RejectedByAdmin = false;
            attraction.RejectedMessage = string.Empty;

            await _context.SaveChangesAsync();

            return await AllRejectedObjects();
        }

        public async Task<IActionResult> VerifyRejectedAccomodation(int verifyAccomodationId)
        {
            var accomodation = _context.Accomodation.Find(verifyAccomodationId);

            accomodation.VerifiedByAdmin = true;
            accomodation.RejectedByAdmin = false;
            accomodation.RejectedMessage = string.Empty;

            await _context.SaveChangesAsync();

            return await AllRejectedObjects();
        }

        public async Task<IActionResult> VerifyAirTicketEC(int verifyAirTicketId)
        {
            var airticket = _context.AirTicket.Find(verifyAirTicketId);

            airticket.VerifiedByAdminECTicket = true;

            await _context.SaveChangesAsync();

            return await AllUnverifiedObjects();
        }

        public async Task<IActionResult> VerifyAirTicketECBooking(int verifyAirTicketId)
        {
            var airticket = _context.BookingAirTickets.Find(verifyAirTicketId);

            airticket.VerifiedBooking = true;
            airticket.RejectedMessage = string.Empty;
            airticket.RejectedBooking = false;
            airticket.CanceledBooking = false;

            await _context.SaveChangesAsync();

            return await BookingsForVerification();
        }

        public async Task<IActionResult> VerifyRejectedAirTicketEC(int verifyAirTicketId)
        {
            var airticket = _context.AirTicket.Find(verifyAirTicketId);

            airticket.VerifiedByAdminECTicket = true;
            airticket.RejectedByAdminEC = false;
            airticket.RejectedMessageEC = string.Empty;

            await _context.SaveChangesAsync();

            return await AllRejectedObjects();
        }

        public async Task<IActionResult> VerifyAirTicketBC(int verifyAirTicketId)
        {
            var airticket = _context.AirTicket.Find(verifyAirTicketId);

            airticket.VerifiedByAdminBCTicket = true;

            await _context.SaveChangesAsync();

            return await AllUnverifiedObjects();
        }

        public async Task<IActionResult> VerifyAirTicketBCBooking(int verifyAirTicketId)
        {
            var airticket = _context.BookingAirTickets.Find(verifyAirTicketId);

            airticket.VerifiedBooking = true;
            airticket.RejectedMessage = string.Empty;
            airticket.RejectedBooking = false;
            airticket.CanceledBooking = false;

            await _context.SaveChangesAsync();

            return await BookingsForVerification();
        }

        public async Task<IActionResult> VerifyRejectedAirTicketBC(int verifyAirTicketId)
        {
            var airticket = _context.AirTicket.Find(verifyAirTicketId);

            airticket.VerifiedByAdminBCTicket = true;
            airticket.RejectedByAdminBC = false;
            airticket.RejectedMessageBC = string.Empty;

            await _context.SaveChangesAsync();

            return await AllRejectedObjects();
        }

        public async Task<IActionResult> VerifyAirTicketFC(int verifyAirTicketId)
        {
            var airticket = _context.AirTicket.Find(verifyAirTicketId);

            airticket.VerifiedByAdminFCTicket = true;

            await _context.SaveChangesAsync();

            return await AllRejectedObjects();
        }

        public async Task<IActionResult> VerifyAirTicketFCBooking(int verifyAirTicketId)
        {
            var airticket = _context.BookingAirTickets.Find(verifyAirTicketId);

            airticket.VerifiedBooking = true;
            airticket.RejectedMessage = string.Empty;
            airticket.RejectedBooking = false;
            airticket.CanceledBooking = false;

            await _context.SaveChangesAsync();

            return await BookingsForVerification();
        }

        public async Task<IActionResult> VerifyRejectedAirTicketFC(int verifyAirTicketId)
        {
            var airticket = _context.AirTicket.Find(verifyAirTicketId);

            airticket.VerifiedByAdminFCTicket = true;
            airticket.RejectedByAdminFC = false;
            airticket.RejectedMessageFC = string.Empty;

            await _context.SaveChangesAsync();

            return await AllRejectedObjects();
        }

        public async Task<IActionResult> RejectCar(int rejectCarId, IFormCollection form)
        {
            var car = _context.Car.Find(rejectCarId);
            var message = form["RejectedMessageCar"];

            car.RejectedByAdmin = true;
            car.RejectedMessage = message;

            await _context.SaveChangesAsync();

            return await AllUnverifiedObjects();
        }

        public async Task<IActionResult> RejectCarBooking(int rejectCarId, IFormCollection form)
        {
            var car = _context.BookingCars.Find(rejectCarId);
            var message = form["RejectedMessageCar"];

            car.RejectedBooking = true;
            car.RejectedMessage = message;
            DateTime startDate = car.DateOfDeparture;
            DateTime endDate = car.ReturnDate;

            int carId = car.CarId;
            var originalCar = _context.Car.Find(carId);
            originalCar.StartDates.Add(startDate);
            originalCar.EndDates.Add(endDate);
            _context.Car.Update(originalCar);

            car.CanceledBooking = true;
            await _context.SaveChangesAsync();


            return await BookingsForVerification();
        }

        public async Task<IActionResult> RejectAttraction(int rejectAttractionId, IFormCollection form)
        {
            var attraction = _context.Entertainment.Find(rejectAttractionId);
            var message = form["RejectedMessageAttraction"];

            attraction.RejectedByAdmin = true;
            attraction.RejectedMessage = message;

            await _context.SaveChangesAsync();

            return await AllUnverifiedObjects();
        }

        public async Task<IActionResult> RejectAttractionBooking(int rejectAttractionId, IFormCollection form)
        {
            var attraction = _context.BookingAttractions.Find(rejectAttractionId);
            var message = form["RejectedMessageAttraction"];

            attraction.RejectedBooking = true;
            attraction.RejectedMessage = message;

            DateTime date = attraction.Date;
            int amountOfTicket = attraction.AmountOfTickets;

            int attractionId = attraction.AttractionId;
            var originalAttraction = _context.Entertainment.Find(attractionId);
            var dates = originalAttraction.AvailableDates;
            int i = 0;
            foreach (var datee in dates)
            {
                if (datee == date)
                {
                    originalAttraction.AmountOfTickets[i] += amountOfTicket;
                }
                i++;
            }
            _context.Entertainment.Update(originalAttraction);

            attraction.CanceledBooking = true;
            await _context.SaveChangesAsync();

            return await BookingsForVerification();
        }

        public async Task<IActionResult> RejectAccomodation(int rejectAccomodationId, IFormCollection form)
        {
            var accomodation = _context.Accomodation.Find(rejectAccomodationId);
            var message = form["RejectedMessageAccomodation"];

            accomodation.RejectedByAdmin = true;
            accomodation.RejectedMessage = message;

            await _context.SaveChangesAsync();

            return await AllUnverifiedObjects();
        }

        public async Task<IActionResult> RejectAccomodationBooking(int rejectAccomodationId, IFormCollection form)
        {
            var accomodation = _context.BookingAccomodations.Find(rejectAccomodationId);
            var message = form["RejectedMessageAccomodation"];

            accomodation.RejectedBooking = true;
            accomodation.RejectedMessage = message;

            int roomId = accomodation.RoomId;
            int rooms = accomodation.Adults;
            DateTime startDate = accomodation.DateOfDeparture;
            DateTime endDate = accomodation.ReturnDate;

            int accomodationId = accomodation.AccomodationId;
            var room = _context.Rooms.Find(roomId);
            var dates = room.AvailableDatesRoom;
            int i = 0;
            foreach (var date in dates)
            {
                if (date >= startDate && date < endDate)
                {
                    room.AmountOfAvailableSameRooms[i] += rooms;
                }
                i++;
            }
            _context.Rooms.Update(room);

            accomodation.CanceledBooking = true;
            await _context.SaveChangesAsync();

            return await BookingsForVerification();
        }

        public async Task<IActionResult> AllRejectedObjects()
        {
            var rejectedCars = await _context.Car
           .Where(c => !c.VerifiedByAdmin && c.RejectedByAdmin)
           .ToListAsync();

            var rejectedAirTickets = await _context.AirTicket
           .Where(c => ((!c.VerifiedByAdminECTicket && c.RejectedByAdminEC) || (!c.VerifiedByAdminBCTicket && c.RejectedByAdminBC) || (!c.VerifiedByAdminFCTicket && c.RejectedByAdminFC)))
           .ToListAsync();

            var rejectedAttractions = await _context.Entertainment
           .Where(c => !c.VerifiedByAdmin && c.RejectedByAdmin)
           .ToListAsync();

            var rejectedAccomodation = await _context.Accomodation
          .Where(c => !c.VerifiedByAdmin && c.RejectedByAdmin)
          .ToListAsync();

            var model = new RejectedObjectsViewModel
            {
                RejectedCars = rejectedCars,
                RejectedAirTickets = rejectedAirTickets,
                RejectedAttractions = rejectedAttractions,
                RejectedAccomodation = rejectedAccomodation
            };

            return View("RejectedItems", model);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteAirTicketECAllRejected(int deleteAirTicketECId)
        {
            var airticket = _context.AirTicket.Find(deleteAirTicketECId); // Находим автомобиль по его ID
            airticket.AmountOfTicketsEC = 0;
            airticket.RejectedMessageEC = string.Empty;

            if (airticket.AmountOfTicketsEC == 0 && airticket.AmountOfTicketsBC == 0 && airticket.AmountOfTicketsFC == 0)
            {
                _context.AirTicket.Remove(airticket); // Удаляем автомобиль из контекста
            }
            await _context.SaveChangesAsync(); // Сохраняем изменения в базе данных
            return await AllRejectedObjects();
        }

        [HttpPost]
        public async Task<IActionResult> DeleteAirTicketBCAllRejected(int deleteAirTicketBCId)
        {
            var airticket = _context.AirTicket.Find(deleteAirTicketBCId); // Находим автомобиль по его ID
            airticket.AmountOfTicketsBC = 0;
            airticket.RejectedMessageBC = string.Empty;

            if (airticket.AmountOfTicketsEC == 0 && airticket.AmountOfTicketsBC == 0 && airticket.AmountOfTicketsFC == 0)
            {
                _context.AirTicket.Remove(airticket); // Удаляем автомобиль из контекста
            }
            await _context.SaveChangesAsync(); // Сохраняем изменения в базе данных
            return await AllRejectedObjects();
        }

        [HttpPost]
        public async Task<IActionResult> DeleteAirTicketFCAllRejected(int deleteAirTicketFCId)
        {
            var airticket = _context.AirTicket.Find(deleteAirTicketFCId); // Находим автомобиль по его ID
            airticket.AmountOfTicketsFC = 0;
            airticket.RejectedMessageFC = string.Empty;

            if (airticket.AmountOfTicketsEC == 0 && airticket.AmountOfTicketsBC == 0 && airticket.AmountOfTicketsFC == 0)
            {
                _context.AirTicket.Remove(airticket); // Удаляем автомобиль из контекста
            }
            await _context.SaveChangesAsync(); // Сохраняем изменения в базе данных
            return await AllRejectedObjects();
        }

        [HttpPost]
        public async Task<IActionResult> DeleteCarAllRejected(int deleteCarId)
        {
            var car = _context.Car.Find(deleteCarId); // Находим автомобиль по его ID

            if (car != null)
            {
                _context.Car.Remove(car); // Удаляем автомобиль из контекста
                _context.SaveChanges(); // Сохраняем изменения в базе данных
            }

            return await AllRejectedObjects();
        }

        [HttpPost]
        public async Task<IActionResult> DeleteAttractionAllRejected(int deleteAttractionId)
        {
            var attraction = _context.Entertainment.Find(deleteAttractionId);
            var objectsToDelete = _context.ObjectPhotos.Where(o => o.ObjectId == deleteAttractionId).ToList();

            if (objectsToDelete.Any())
            {
                // Удаляем найденные объекты
                _context.ObjectPhotos.RemoveRange(objectsToDelete);
                await _context.SaveChangesAsync();
            }

            if (attraction != null)
            {
                _context.Entertainment.Remove(attraction); 
                _context.SaveChanges(); // Сохраняем изменения в базе данных
            }

            return await AllRejectedObjects();
        }

        [HttpPost]
        public async Task<IActionResult> DeleteAccomodationAllRejected(int deleteAccomodationId)
        {
            var accomodation = _context.Accomodation.Find(deleteAccomodationId);
            var objectsToDelete = _context.AccomodationPhotos.Where(o => o.ObjectId == deleteAccomodationId).ToList();

            if (objectsToDelete.Any())
            {
                // Удаляем найденные объекты
                _context.AccomodationPhotos.RemoveRange(objectsToDelete);
                await _context.SaveChangesAsync();
            }

            if (accomodation != null)
            {
                _context.Accomodation.Remove(accomodation);
                _context.SaveChanges(); // Сохраняем изменения в базе данных
            }

            return await AllRejectedObjects();
        }

        public async Task<IActionResult> RejectAirTicketEC(int rejectAirTicketId, IFormCollection form)
        {
            var airticket = _context.AirTicket.Find(rejectAirTicketId);
            var message = form["RejectedMessageAirTicketEC"];

            airticket.RejectedByAdminEC = true;
            airticket.RejectedMessageEC = message;

            await _context.SaveChangesAsync();

            return await AllUnverifiedObjects();
        }

        public async Task<IActionResult> RejectAirTicketECBooking(int rejectAirTicketId, IFormCollection form)
        {
            var airticket = _context.BookingAirTickets.Find(rejectAirTicketId);
            var message = form["RejectedMessageAirTicketEC"];

            airticket.RejectedBooking = true;
            airticket.RejectedMessage = message;
            int passengers = airticket.Passengers;

            int airticketId = airticket.AirTicketId;
            var originalAirticket = _context.AirTicket.Find(airticketId);
            originalAirticket.AmountOfTicketsEC += passengers;
            _context.AirTicket.Update(originalAirticket);

            airticket.CanceledBooking = true;
            await _context.SaveChangesAsync();

            return await BookingsForVerification();
        }

        public async Task<IActionResult> RejectAirTicketBC(int rejectAirTicketId, IFormCollection form)
        {
            var airticket = _context.AirTicket.Find(rejectAirTicketId);
            var message = form["RejectedMessageAirTicketBC"];

            airticket.RejectedByAdminBC = true;
            airticket.RejectedMessageBC = message;

            await _context.SaveChangesAsync();

            return await AllUnverifiedObjects();
        }

        public async Task<IActionResult> RejectAirTicketBCBooking(int rejectAirTicketId, IFormCollection form)
        {
            var airticket = _context.BookingAirTickets.Find(rejectAirTicketId);
            var message = form["RejectedMessageAirTicketBC"];

            airticket.RejectedBooking = true;
            airticket.RejectedMessage = message;
            int passengers = airticket.Passengers;

            int airticketId = airticket.AirTicketId;
            var originalAirticket = _context.AirTicket.Find(airticketId);
            originalAirticket.AmountOfTicketsBC += passengers;
            _context.AirTicket.Update(originalAirticket);

            airticket.CanceledBooking = true;
            await _context.SaveChangesAsync();

            return await BookingsForVerification();
        }

        public async Task<IActionResult> RejectAirTicketFC(int rejectAirTicketId, IFormCollection form)
        {
            var airticket = _context.AirTicket.Find(rejectAirTicketId);
            var message = form["RejectedMessageAirTicketFC"];

            airticket.RejectedByAdminFC = true;
            airticket.RejectedMessageFC = message;

            await _context.SaveChangesAsync();

            return await AllUnverifiedObjects();
        }

        public async Task<IActionResult> RejectAirTicketFCBooking(int rejectAirTicketId, IFormCollection form)
        {
            var airticket = _context.BookingAirTickets.Find(rejectAirTicketId);
            var message = form["RejectedMessageAirTicketFC"];

            airticket.RejectedBooking = true;
            airticket.RejectedMessage = message;
            int passengers = airticket.Passengers;

            int airticketId = airticket.AirTicketId;
            var originalAirticket = _context.AirTicket.Find(airticketId);
            originalAirticket.AmountOfTicketsFC += passengers;
            _context.AirTicket.Update(originalAirticket);

            airticket.CanceledBooking = true;
            await _context.SaveChangesAsync(); 

            return await BookingsForVerification();
        }

        public async Task<IActionResult> AddRoomUnverified(string addRoomAccomodationId)
        {
            var entities = new List<EntityModel>
            {
                new EntityModel { Name = "Accommodation", Description = "Description for Accommodation" },
                new EntityModel { Name = "Flight", Description = "Description for Flight" },
                new EntityModel { Name = "Car", Description = "Description for Car" },
                new EntityModel { Name = "Attraction", Description = "Description for Attraction" }
            };

            ViewBag.addRoomAccomodationId = addRoomAccomodationId;

            return View("AddRoomPage", entities);
        }

        [HttpPost]
        public async Task<IActionResult> SaveRoomForm(Room model, string photoData, int addRoomAccomodationId)
        {
            var photos = JsonConvert.DeserializeObject<List<string>>(photoData);
            var photoBytesList = photos.Select(photoBase64 => Convert.FromBase64String(photoBase64.Split(',')[1])).ToList();
            var currentUser = await _context.Users.FirstOrDefaultAsync(u => u.UserName == User.Identity.Name);
            var currentAccomodation = _context.Accomodation.Find(addRoomAccomodationId);

            var entities = new List<EntityModel>
            {
                new EntityModel { Name = "Accommodation", Description = "Description for Accommodation" },
                new EntityModel { Name = "Flight", Description = "Description for Flight" },
                new EntityModel { Name = "Car", Description = "Description for Car" },
                new EntityModel { Name = "Attraction", Description = "Description for Attraction" }
            };

            if(currentAccomodation.MinCost > model.RoomCost)
            {
                currentAccomodation.MinCost = model.RoomCost;
            }

            var room = new Room
            {
                RoomName = model.RoomName,
                WashingMachine = model.WashingMachine,
                Kitchen = model.Kitchen,
                WheelchairAccessibleRoom = model.WheelchairAccessibleRoom,
                ToiletWithGrabBars = model.ToiletWithGrabBars,
                BathtubWithgrabbars = model.BathtubWithgrabbars,
                BarrierFreeShower = model.BarrierFreeShower,
                ShowerWithoutEdge = model.ShowerWithoutEdge,
                HighToilet = model.HighToilet,
                LowSink = model.LowSink,
                MainPhoto = photoBytesList[0],
                BathroomEmergencyButton = model.BathroomEmergencyButton,
                ShowerChair = model.ShowerChair,
                TypeOfNutritionRoom = model.TypeOfNutritionRoom,
                CoffeeMachine = model.CoffeeMachine,
                CoffeeOrTea = model.CoffeeOrTea,
                ElectricKettle = model.ElectricKettle,
                View = model.View,
                Soundproofing = model.Soundproofing,
                Patio = model.Patio,
                FlatScreenTV = model.FlatScreenTV,
                Balcony = model.Balcony,
                Terrace = model.Terrace,
                PrivatePool = model.PrivatePool,
                Bath = model.Bath,
                PlaceToWorkOnALaptop = model.PlaceToWorkOnALaptop,
                AirConditioner = model.AirConditioner,
                PrivateBathroom = model.PrivateBathroom,
                FreeCancellation = model.FreeCancellation,
                RoomDescription = model.RoomDescription,
                AvailableDatesRoom = model.AvailableDatesRoom,
                AmountOfAvailableSameRooms = model.AmountOfAvailableSameRooms,
                RoomCost = model.RoomCost,
                AccomodationId = currentAccomodation.Id
            };

            string filePath = "C:\\Users\\Angelina\\Pictures\\Camera Roll\\bbb846030d108b92a3fefbfca1f7bbe6.jpg";
            byte[] fileBytes = System.IO.File.ReadAllBytes(filePath);
            ViewBag.CarPhoto = fileBytes;
            // добавить сохранение дат в другую БД
            _context.Rooms.Add(room);
            await _context.SaveChangesAsync();

            currentAccomodation.AllRooms.Add(room);
            currentAccomodation.VerifiedByAdmin = false;
            currentAccomodation.RejectedByAdmin = false;
            currentAccomodation.RejectedMessage = string.Empty;

            foreach (var photo in photoBytesList)
            {
                var photoss = new RoomPhotos
                {
                    ObjectId = currentAccomodation.Id,
                    PhotoArray = photo,
                    RoomId = room.ID
                };
                _context.RoomPhotos.Add(photoss);
                await _context.SaveChangesAsync();
            }

            return await UnverifiedObjects();
        }

        [HttpPost]
        public async Task<IActionResult> DeleteRoom(int deleteRoomId, int deleteRoomAccomodationId)
        {
            var room = _context.Rooms.Find(deleteRoomId);
            var accomodation = _context.Accomodation.Find(deleteRoomAccomodationId);
            accomodation.VerifiedByAdmin = false;
            accomodation.RejectedMessage = string.Empty;
            accomodation.RejectedByAdmin = false;
            var objectsToDelete = _context.RoomPhotos.Where(o => o.RoomId == deleteRoomId && o.ObjectId == deleteRoomAccomodationId).ToList();

            if (objectsToDelete.Any())
            {
                // Удаляем найденные объекты
                _context.RoomPhotos.RemoveRange(objectsToDelete);
                await _context.SaveChangesAsync();
            }

            if (room != null)
            {
                _context.Rooms.Remove(room); // Удаляем автомобиль из контекста
                _context.SaveChanges(); // Сохраняем изменения в базе данных
            }

            return await ShowAccomodationInformation(deleteRoomAccomodationId);
        }

        [HttpPost]
        public async Task<IActionResult> EditRoom(int editRoomId, int editRoomAccomodationId)
        {
            var room = _context.Rooms.Find(editRoomId);
            var accomodation = _context.Accomodation.Find(editRoomAccomodationId);
            var objects = _context.RoomPhotos.Where(o => o.RoomId == editRoomId && o.ObjectId == editRoomAccomodationId).Select(o => o.PhotoArray).ToList();

            ViewBag.RoomPhotos = objects;
            ViewBag.RoomName = room.RoomName;
            ViewBag.WashingMachine = room.WashingMachine;
            ViewBag.Kitchen = room.Kitchen;
            ViewBag.WheelchairAccessibleRoom = room.WheelchairAccessibleRoom;
            ViewBag.ToiletWithGrabBars = room.ToiletWithGrabBars;
            ViewBag.BarrierFreeShower = room.BarrierFreeShower;
            ViewBag.BathtubWithgrabbars = room.BathtubWithgrabbars;
            ViewBag.ShowerWithoutEdge = room.ShowerWithoutEdge;
            ViewBag.HighToilet = room.HighToilet;
            ViewBag.LowSink = room.LowSink;
            ViewBag.BathroomEmergencyButton = room.BathroomEmergencyButton;
            ViewBag.ShowerChair = room.ShowerChair;
            ViewBag.Nutrition = room.TypeOfNutritionRoom;
            ViewBag.CoffeeMachine = room.CoffeeMachine;
            ViewBag.CoffeeOrTea = room.CoffeeOrTea;
            ViewBag.ElectricKettle = room.ElectricKettle;
            ViewBag.View = room.View;
            ViewBag.Soundproofing = room.Soundproofing;
            ViewBag.Patio = room.Patio;
            ViewBag.FlatScreenTV = room.FlatScreenTV;
            ViewBag.Balcony = room.Balcony;
            ViewBag.Terrace = room.Terrace;
            ViewBag.PrivatePool = room.PrivatePool;
            ViewBag.Bath = room.Bath;
            ViewBag.PlaceToWorkOnALaptop = room.PlaceToWorkOnALaptop;
            ViewBag.AirConditioner = room.AirConditioner;
            ViewBag.PrivateBathroom = room.PrivateBathroom;
            ViewBag.FreeCancellation = room.FreeCancellation;
            ViewBag.RoomDescription = room.RoomDescription;

            List<DateTime> Dates = room.AvailableDatesRoom;
            List<int> AmountOfSameRooms = room.AmountOfAvailableSameRooms;

            ViewBag.Dates = Dates;
            ViewBag.AmountOfSameRooms = AmountOfSameRooms;

            ViewBag.RoomCost = room.RoomCost;

            ViewBag.editRoomId = room.ID;
            ViewBag.editRoomAccomodationId = room.AccomodationId;

            return View("EditRoomPage");
        }

        [HttpPost]
        public async Task<IActionResult> UpdateRoomForm(int editRoomId, int editRoomAccomodationId, Room model, string photoDataRoom)
        {
            var currentUser = await _context.Users.FirstOrDefaultAsync(u => u.UserName == User.Identity.Name);

            var accomodation = _context.Accomodation.Find(editRoomAccomodationId);
            var room = _context.Rooms.Find(editRoomId);
            accomodation.VerifiedByAdmin = false;
            accomodation.RejectedMessage = string.Empty;
            accomodation.RejectedByAdmin = false;
            var objectsToDelete = _context.RoomPhotos.Where(o => o.RoomId == editRoomId && o.ObjectId == editRoomAccomodationId).ToList();

            if (objectsToDelete.Any())
            {
                // Удаляем найденные объекты
                _context.RoomPhotos.RemoveRange(objectsToDelete);
                await _context.SaveChangesAsync();
            }

            string filePath = "C:\\Users\\Angelina\\Pictures\\Camera Roll\\bbb846030d108b92a3fefbfca1f7bbe6.jpg";
            byte[] fileBytes = System.IO.File.ReadAllBytes(filePath);
            ViewBag.CarPhoto = fileBytes;

            var photos = JsonConvert.DeserializeObject<List<string>>(photoDataRoom);
            var photoBytesList = photos.Select(photoBase64 => Convert.FromBase64String(photoBase64.Split(',')[1])).ToList();

            room.RoomName = model.RoomName;
            room.WashingMachine = model.WashingMachine;
            room.Kitchen = model.Kitchen;
            room.WheelchairAccessibleRoom = model.WheelchairAccessibleRoom;
            room.ToiletWithGrabBars = model.ToiletWithGrabBars;
            room.BathtubWithgrabbars = model.BathtubWithgrabbars;
            room.BarrierFreeShower = model.BarrierFreeShower;
            room.ShowerWithoutEdge = model.ShowerWithoutEdge;
            room.HighToilet = model.HighToilet;
            room.LowSink = model.LowSink;
            room.MainPhoto = photoBytesList[0];
            room.BathroomEmergencyButton = model.BathroomEmergencyButton;
            room.ShowerChair = model.ShowerChair;
            room.TypeOfNutritionRoom = model.TypeOfNutritionRoom;
            room.CoffeeMachine = model.CoffeeMachine;
            room.CoffeeOrTea = model.CoffeeOrTea;
            room.ElectricKettle = model.ElectricKettle;
            room.View = model.View;
            room.Soundproofing = model.Soundproofing;
            room.Patio = model.Patio;
            room.FlatScreenTV = model.FlatScreenTV;
            room.Balcony = model.Balcony;
            room.Terrace = model.Terrace;
            room.PrivatePool = model.PrivatePool;
            room.Bath = model.Bath;
            room.PlaceToWorkOnALaptop = model.PlaceToWorkOnALaptop;
            room.AirConditioner = model.AirConditioner;
            room.PrivateBathroom = model.PrivateBathroom;
            room.FreeCancellation = model.FreeCancellation;
            room.RoomDescription = model.RoomDescription;
            room.AvailableDatesRoom = model.AvailableDatesRoom;
            room.AmountOfAvailableSameRooms = model.AmountOfAvailableSameRooms;
            room.RoomCost = model.RoomCost;
            room.AccomodationId = editRoomAccomodationId;

            accomodation.MinCost = room.RoomCost;

            await _context.SaveChangesAsync();

            foreach (var photo in photoBytesList)
            {
                var photoss = new RoomPhotos
                {
                    ObjectId = editRoomAccomodationId,
                    PhotoArray = photo,
                    RoomId = editRoomId
                };
                _context.RoomPhotos.Add(photoss);
                await _context.SaveChangesAsync();
            }

            await _context.SaveChangesAsync(); // Сохраняем изменения в базе данных
            return await ShowAccomodationInformation(editRoomAccomodationId);
        }

        [HttpPost]
        public async Task<IActionResult> CancelCarBooking(int cancelCarId)
        {
            var car = _context.BookingCars.Find(cancelCarId);
            DateTime startDate = car.DateOfDeparture;
            DateTime endDate = car.ReturnDate;

            int carId = car.CarId;
            var originalCar = _context.Car.Find(carId);
            originalCar.StartDates.Add(startDate);
            originalCar.EndDates.Add(endDate);
            _context.Car.Update(originalCar);

            car.CanceledBooking = true;
            await _context.SaveChangesAsync();

            return await ShowUnverifiedBookingItems();
        }

        [HttpPost]
        public async Task<IActionResult> CancelCarBookingg(int cancelCarId)
        {
            var car = _context.BookingCars.Find(cancelCarId);
            DateTime startDate = car.DateOfDeparture;
            DateTime endDate = car.ReturnDate;

            int carId = car.CarId;
            var originalCar = _context.Car.Find(carId);
            originalCar.StartDates.Add(startDate);
            originalCar.EndDates.Add(endDate);
            _context.Car.Update(originalCar);

            car.CanceledBooking = true;
            car.VerifiedBooking = false;
            await _context.SaveChangesAsync();

            return await ShowBookingItems();
        }

        [HttpPost]
        public async Task<IActionResult> CancelAttractionBooking(int cancelAttractionId)
        {
            var attraction = _context.BookingAttractions.Find(cancelAttractionId);
            DateTime date = attraction.Date;
            int amountOfTicket = attraction.AmountOfTickets;

            int attractionId = attraction.AttractionId;
            var originalAttraction = _context.Entertainment.Find(attractionId);
            var dates = originalAttraction.AvailableDates;
            int i = 0;
            foreach (var datee in dates)
            {
                if(datee == date)
                {
                    originalAttraction.AmountOfTickets[i] += amountOfTicket;
                }
                i++;
            }
            _context.Entertainment.Update(originalAttraction);

            attraction.CanceledBooking = true;
            await _context.SaveChangesAsync();

            return await ShowUnverifiedBookingItems();
        }

        [HttpPost]
        public async Task<IActionResult> CancelAttractionBookingg(int cancelAttractionId)
        {
            var attraction = _context.BookingAttractions.Find(cancelAttractionId);
            DateTime date = attraction.Date;
            int amountOfTicket = attraction.AmountOfTickets;

            int attractionId = attraction.AttractionId;
            var originalAttraction = _context.Entertainment.Find(attractionId);
            var dates = originalAttraction.AvailableDates;
            int i = 0;
            foreach (var datee in dates)
            {
                if (datee == date)
                {
                    originalAttraction.AmountOfTickets[i] += amountOfTicket;
                }
                i++;
            }
            _context.Entertainment.Update(originalAttraction);

            attraction.CanceledBooking = true;
            attraction.VerifiedBooking = false;
            await _context.SaveChangesAsync();

            return await ShowBookingItems();
        }

        [HttpPost]
        public async Task<IActionResult> CancelAccomodationBooking(int cancelAccomodationId)
        {
            var accomodation = _context.BookingAccomodations.Find(cancelAccomodationId);
            int roomId = accomodation.RoomId;
            int rooms = accomodation.Adults;
            DateTime startDate = accomodation.DateOfDeparture;
            DateTime endDate = accomodation.ReturnDate;

            int accomodationId = accomodation.AccomodationId;
            var room = _context.Rooms.Find(roomId);
            var dates = room.AvailableDatesRoom;
            int i = 0;
            foreach(var date in dates)
            {
                if(date >= startDate && date < endDate)
                {
                    room.AmountOfAvailableSameRooms[i] += rooms;
                }
                i++;
            }
            _context.Rooms.Update(room);

            accomodation.CanceledBooking = true;
            await _context.SaveChangesAsync();

            return await ShowUnverifiedBookingItems();
        }

        [HttpPost]
        public async Task<IActionResult> CancelAccomodationBookingg(int cancelAccomodationId)
        {
            var accomodation = _context.BookingAccomodations.Find(cancelAccomodationId);
            int roomId = accomodation.RoomId;
            int rooms = accomodation.Adults;
            DateTime startDate = accomodation.DateOfDeparture;
            DateTime endDate = accomodation.ReturnDate;

            int accomodationId = accomodation.AccomodationId;
            var room = _context.Rooms.Find(roomId);
            var dates = room.AvailableDatesRoom;
            int i = 0;
            foreach (var date in dates)
            {
                if (date >= startDate && date < endDate)
                {
                    room.AmountOfAvailableSameRooms[i] += rooms;
                }
                i++;
            }
            _context.Rooms.Update(room);

            accomodation.CanceledBooking = true;
            accomodation.VerifiedBooking = false;
            await _context.SaveChangesAsync();

            return await ShowBookingItems();
        }

        [HttpPost]
        public async Task<IActionResult> CancelAirTicketECBooking(int cancelAirTicketId)
        {
            var airticket = _context.BookingAirTickets.Find(cancelAirTicketId);
            int passengers = airticket.Passengers;

            int airticketId = airticket.AirTicketId;
            var originalAirticket = _context.AirTicket.Find(airticketId);
            originalAirticket.AmountOfTicketsEC += passengers;
            _context.AirTicket.Update(originalAirticket);

            airticket.CanceledBooking = true;
            await _context.SaveChangesAsync();

            return await ShowUnverifiedBookingItems();
        }

        [HttpPost]
        public async Task<IActionResult> CancelAirTicketECBookingg(int cancelAirTicketId)
        {
            var airticket = _context.BookingAirTickets.Find(cancelAirTicketId);
            int passengers = airticket.Passengers;

            int airticketId = airticket.AirTicketId;
            var originalAirticket = _context.AirTicket.Find(airticketId);
            originalAirticket.AmountOfTicketsEC += passengers;
            _context.AirTicket.Update(originalAirticket);

            airticket.CanceledBooking = true;
            airticket.VerifiedBooking = false;
            await _context.SaveChangesAsync();

            return await ShowBookingItems();
        }

        [HttpPost]
        public async Task<IActionResult> CancelAirTicketBCBooking(int cancelAirTicketId)
        {
            var airticket = _context.BookingAirTickets.Find(cancelAirTicketId);
            int passengers = airticket.Passengers;

            int airticketId = airticket.AirTicketId;
            var originalAirticket = _context.AirTicket.Find(airticketId);
            originalAirticket.AmountOfTicketsBC += passengers;
            _context.AirTicket.Update(originalAirticket);

            airticket.CanceledBooking = true;
            await _context.SaveChangesAsync();

            return await ShowUnverifiedBookingItems();
        }

        [HttpPost]
        public async Task<IActionResult> CancelAirTicketBCBookingg(int cancelAirTicketId)
        {
            var airticket = _context.BookingAirTickets.Find(cancelAirTicketId);
            int passengers = airticket.Passengers;

            int airticketId = airticket.AirTicketId;
            var originalAirticket = _context.AirTicket.Find(airticketId);
            originalAirticket.AmountOfTicketsBC += passengers;
            _context.AirTicket.Update(originalAirticket);

            airticket.CanceledBooking = true;
            airticket.VerifiedBooking = false;
            await _context.SaveChangesAsync();

            return await ShowBookingItems();
        }

        [HttpPost]
        public async Task<IActionResult> CancelAirTicketFCBooking(int cancelAirTicketId)
        {
            var airticket = _context.BookingAirTickets.Find(cancelAirTicketId);
            int passengers = airticket.Passengers;

            int airticketId = airticket.AirTicketId;
            var originalAirticket = _context.AirTicket.Find(airticketId);
            originalAirticket.AmountOfTicketsFC += passengers;
            _context.AirTicket.Update(originalAirticket);

            airticket.CanceledBooking = true;
            await _context.SaveChangesAsync();

            return await ShowUnverifiedBookingItems();
        }

        [HttpPost]
        public async Task<IActionResult> CancelAirTicketFCBookingg(int cancelAirTicketId)
        {
            var airticket = _context.BookingAirTickets.Find(cancelAirTicketId);
            int passengers = airticket.Passengers;

            int airticketId = airticket.AirTicketId;
            var originalAirticket = _context.AirTicket.Find(airticketId);
            originalAirticket.AmountOfTicketsFC += passengers;
            _context.AirTicket.Update(originalAirticket);

            airticket.CanceledBooking = true;
            airticket.VerifiedBooking = false;
            await _context.SaveChangesAsync();

            return await ShowBookingItems();
        }

    }
}
