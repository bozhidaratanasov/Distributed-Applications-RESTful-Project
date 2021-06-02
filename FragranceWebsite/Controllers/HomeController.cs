using FragranceWebsite.Models;
using FragranceWebsite.Repository.IRepository;
using FragranceWebsite.ViewModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace FragranceWebsite.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IFragranceRepository _fragranceRepo;
        private readonly IUserRepository _userRepo;

        public HomeController(ILogger<HomeController> logger, IFragranceRepository fragranceRepo, IUserRepository userRepo)
        {
            _logger = logger;
            _fragranceRepo = fragranceRepo;
            _userRepo = userRepo;
        }

        public async Task<IActionResult> Index()
        {
            IndexVM frList = new IndexVM()
            {
                FragranceList = (IEnumerable<FragranceVM>)await _fragranceRepo.GetAllAsync(StaticInfo.FragranceAPIPath, HttpContext.Session.GetString("JWToken"))
            };
            return View(frList);
        }

        [HttpGet]
        public IActionResult Login()
        {
            UserVM userVM = new UserVM();
            return View(userVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(UserVM userVM)
        {
            userVM = await _userRepo.LoginAsync(StaticInfo.UserAPIPath + "authenticate", userVM);
            if (userVM.Token == null)
                return View();

            var identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme);
            identity.AddClaim(new Claim(ClaimTypes.Name, userVM.Username));
            identity.AddClaim(new Claim(ClaimTypes.Role, userVM.Role));
            var principal = new ClaimsPrincipal(identity);
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

            HttpContext.Session.SetString("JWToken", userVM.Token);
            TempData["alert"] = "Welcome " + userVM.Username;
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(UserVM userVM)
        {
            bool result = await _userRepo.RegisterAsync(StaticInfo.UserAPIPath + "register", userVM);
            if (!result)
                return View();

            TempData["alert"] = "Registration Successful";
            return RedirectToAction("Login");
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            HttpContext.Session.SetString("JWToken", "");
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult AccessDenied()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
