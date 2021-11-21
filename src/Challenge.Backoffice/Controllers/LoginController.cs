using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Challenge.Application.Business.Users.Queries;
using Challenge.Common;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Challenge.Backoffice.Controllers
{
    public class LoginController : Controller
    {
        private readonly Dispatcher _dispatcher;

        public LoginController(Dispatcher dispatcher)
        {
            _dispatcher = dispatcher;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index([FromForm] SignInAdminQuery query)
        {
            if (!ModelState.IsValid)
                return View(query);

            var user = await _dispatcher.Dispatch(query);
            if (user == null)
            {
                TempData["Error"] = "Hatalı e-posta veya parola girdiniz.";
                return View(query);
            }

            var model = new VerifyPhoneNumberAdminQuery { Email = query.Email, IsPersistent = false };

            return View("VerifyPhoneNumber", model);
        }

        [HttpPost]
        public async Task<IActionResult> VerifyPhoneNumber([FromForm] VerifyPhoneNumberAdminQuery query, string returnUrl)
        {
            var result = await _dispatcher.Dispatch(query);
            if (result.HasError)
            {
                TempData["Error"] = result.Message;
                return View(query);
            }

            var claims = new Claim[]
            {
                new Claim("UserId", result.User.Id),
                new Claim("Email", result.User.Email),
                new Claim("FullName", result.User.FirstName + " " + result.User.LastName),
                new Claim(ClaimTypes.Role, result.User.Role.ToString()),
            };

            ClaimsIdentity userIdentity = new ClaimsIdentity(claims, "login");
            ClaimsPrincipal principal = new ClaimsPrincipal(userIdentity);

            await HttpContext.SignInAsync(principal, new AuthenticationProperties
            {
                IsPersistent = query.IsPersistent,
                ExpiresUtc = DateTime.UtcNow.AddDays(30)
            });

            if (!string.IsNullOrWhiteSpace(returnUrl))
                return LocalRedirect(returnUrl);

            return RedirectToAction("Index", "Home");
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}