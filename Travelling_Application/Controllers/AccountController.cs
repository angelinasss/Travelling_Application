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
         
            // Преобразовать модель представления в объект Car и сохранить его в базе данных
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
        public async Task<IActionResult> SaveEntertainmentForm(Entertainment model, IFormFileCollection photos)
        {
            var currentUser = await _context.Users.FirstOrDefaultAsync(u => u.UserName == User.Identity.Name);

            var entities = new List<EntityModel>
                {
                new EntityModel { Name = "Accommodation", Description = "Description for Accommodation" },
                new EntityModel { Name = "Flight", Description = "Description for Flight" },
                new EntityModel { Name = "Car", Description = "Description for Car" },
                new EntityModel { Name = "Attraction", Description = "Description for Attraction" }
                };

            if (photos.Count > 0)
            {
                foreach (var photo in photos)
                {
                    // Обработка каждого файла, например, сохранение в базу данных
                }
            }

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
                VerifiedByAdmin = false,
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
           .Where(c => !c.VerifiedByAdmin && !c.RejectedByAdmin && c.PublisherId == currentUser.Id)
           .ToListAsync();

            var model = new UnverifiedObjectsViewModel
            {
                UnverifiedCars = unverifiedCars,
                UnverifiedAirTickets = unverifiedAirTickets
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
           .Where(c => c.VerifiedByAdmin && c.PublisherId == currentUser.Id)
           .ToListAsync();

            var model = new VerifiedObjectsViewModel
            {
                VerifiedCars = verifiedCars,
                VerifiedAirTickets = verifiedAirTickets
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
           .Where(c => !c.VerifiedByAdmin && c.RejectedByAdmin && c.PublisherId == currentUser.Id)
           .ToListAsync();

            var model = new UnverifiedObjectsViewModel
            {
                UnverifiedCars = unverifiedCars,
                UnverifiedAirTickets = unverifiedAirTickets
            };

            return View("RejectedItems", model);
        }
    }
}
