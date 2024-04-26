using Auth0.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;
using Microsoft.EntityFrameworkCore;
using System;
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
                    // добавляем пользователя в бд
                    user = new User { UserName = model.UserName, Password = model.Password, Name = "", Birthday = new DateTime(),
                    Nationality = "", Email = "", PhoneNumber = "", Sex = "", Role = model.AccountType};

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
                    ViewBag.UserPhotoUrl = new byte[10]; // или другое значение по умолчанию
                }
                if (birthday != null)
                {
                    ViewBag.DefaultBirthday = birthday.ToString("yyyy-MM-dd"); ;
                }
                else
                {
                    ViewBag.DefaultBirthday = ""; // или другое значение по умолчанию
                }
                if (sex != null)
                {
                    ViewBag.DefaultSex = sex;
                }
                else
                {
                    ViewBag.DefaultSex = ""; // или другое значение по умолчанию
                }
                if (name != null)
                {
                    ViewBag.DefaultName = name;
                }
                else
                {
                    ViewBag.DefaultName = ""; // или другое значение по умолчанию
                }
                if (email != null)
                {
                    ViewBag.DefaultEmail = email;
                }
                else
                {
                    ViewBag.DefaultEmail = ""; // или другое значение по умолчанию
                }
                if (phoneNumber != null)
                {
                    ViewBag.DefaultPhoneNumber = phoneNumber;
                }
                else
                {
                    ViewBag.DefaultPhoneNumber = ""; // или другое значение по умолчанию
                }
                if (nationality != null)
                {
                    ViewBag.DefaultNationality = nationality;
                }
                else
                {
                    ViewBag.DefaultNationality = ""; // или другое значение по умолчанию
                }
                if (countryCode != null)
                {
                    ViewBag.DefaultCountryCode = countryCode;
                }
                else
                {
                    ViewBag.DefaultCountryCode = ""; // или другое значение по умолчанию
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
    }
}
