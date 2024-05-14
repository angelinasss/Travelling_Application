using Auth0.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Security.Claims;
using Travelling_Application.Models;
using Travelling_Application.ViewModels;
using static System.Runtime.InteropServices.JavaScript.JSType;
using Microsoft.AspNetCore.Http;
using System.Diagnostics.Metrics;

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
            return View("ViewProfile");
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

        public async Task<IActionResult> ShowFavoriteItems()
        {
            return View("FavoriteItems");
        }

        public async Task<IActionResult> ShowBookingItems()
        {
            return View("BookingItems");
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
                PublisherId = currentUser.Id
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
                PublisherId = currentUser.Id
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
                PublisherId = currentUser.Id
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

            var model = new UnverifiedObjectsViewModel
            {
                UnverifiedCars = unverifiedCars,
                UnverifiedAirTickets = unverifiedAirTickets,
                UnverifiedAttractions = unverifiedAttractions
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

            var model = new VerifiedObjectsViewModel
            {
                VerifiedCars = verifiedCars,
                VerifiedAirTickets = verifiedAirTickets,
                VerifiedAttractions = verifiedAttractions
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

            var model = new RejectedObjectsViewModel
            {
                RejectedCars = unverifiedCars,
                RejectedAirTickets = unverifiedAirTickets,
                RejectedAttractions = unverifiedAttractions,
            };

            return View("RejectedItems", model);
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
            //ViewBag.CarPhoto = car.CarPhoto;
            ViewBag.AttractionName = attraction.Title;
            ViewBag.AttractionCountry = attraction.Country;
            ViewBag.AttractionCity = attraction.City;
            ViewBag.AttractionAddress = attraction.Address;
            //ViewBag.ElectricCar = car.ElectricCar;
            //ViewBag.AttractionCategory = attraction.Category;
            ViewBag.TimeOfDay = attraction.TimeOfDay;
            ViewBag.FreeCancellation = attraction.FreeCancellation;
            ViewBag.AttractionDescription = attraction.Description;
            ViewBag.AttractionPrice = attraction.Cost;

            //List<DateTime> DatesStart = car.StartDates;

            //ViewBag.DatesStart = DatesStart;

            //ViewBag.editCarId = car.Id;

            return View("EditAttractionPage");
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

            var model = new VerifiedObjectsViewModel
            {
                VerifiedCars = verifiedCars,
                VerifiedAirTickets = verifiedAirTickets,
                VerifiedAttractions = verifiedAttractions
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

            var model = new UnverifiedObjectsViewModel
            {
                UnverifiedCars = unverifiedCars,
                UnverifiedAirTickets = unverifiedAirTickets,
                UnverifiedAttractions = unverifiedAttractions
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

        public async Task<IActionResult> VerifyAttraction(int verifyAttractionId)
        {
            var attraction = _context.Entertainment.Find(verifyAttractionId);

            attraction.VerifiedByAdmin = true;

            await _context.SaveChangesAsync();

            return await AllUnverifiedObjects();
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

        public async Task<IActionResult> VerifyAirTicketEC(int verifyAirTicketId)
        {
            var airticket = _context.AirTicket.Find(verifyAirTicketId);

            airticket.VerifiedByAdminECTicket = true;

            await _context.SaveChangesAsync();

            return await AllUnverifiedObjects();
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

        public async Task<IActionResult> RejectAttraction(int rejectAttractionId, IFormCollection form)
        {
            var attraction = _context.Entertainment.Find(rejectAttractionId);
            var message = form["RejectedMessageAttraction"];

            attraction.RejectedByAdmin = true;
            attraction.RejectedMessage = message;

            await _context.SaveChangesAsync();

            return await AllUnverifiedObjects();
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

            var model = new RejectedObjectsViewModel
            {
                RejectedCars = rejectedCars,
                RejectedAirTickets = rejectedAirTickets,
                RejectedAttractions = rejectedAttractions
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

        public async Task<IActionResult> RejectAirTicketEC(int rejectAirTicketId, IFormCollection form)
        {
            var airticket = _context.AirTicket.Find(rejectAirTicketId);
            var message = form["RejectedMessageAirTicketEC"];

            airticket.RejectedByAdminEC = true;
            airticket.RejectedMessageEC = message;

            await _context.SaveChangesAsync();

            return await AllUnverifiedObjects();
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

        public async Task<IActionResult> RejectAirTicketFC(int rejectAirTicketId, IFormCollection form)
        {
            var airticket = _context.AirTicket.Find(rejectAirTicketId);
            var message = form["RejectedMessageAirTicketFC"];

            airticket.RejectedByAdminFC = true;
            airticket.RejectedMessageFC = message;

            await _context.SaveChangesAsync();

            return await AllUnverifiedObjects();
        }
        //EditAttractionUnverified          
    }
}
