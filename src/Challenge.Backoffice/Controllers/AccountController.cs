using System.Linq;
using System.Threading.Tasks;
using Challenge.Application.Business.Users.Queries;
using Challenge.Application.Services.CurrentUser;
using Challenge.Backoffice.Controllers.Base;
using Challenge.Common;
using Microsoft.AspNetCore.Mvc;

namespace Challenge.Backoffice.Controllers
{
    public class AccountController : BaseController
    {
        private readonly Dispatcher _dispatcher;
        private readonly ICurrentUserService _currentUser;

        public AccountController(Dispatcher dispatcher, ICurrentUserService currentUser)
        {
            _dispatcher = dispatcher;
            _currentUser = currentUser;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var user = await _dispatcher.Dispatch(new GetUserQuery { Id = _currentUser.Id });
            var response = new UpdateAccountAdminQuery
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber
            };

            return View(response);
        }

        [HttpPost]
        public async Task<IActionResult> Index([FromForm] UpdateAccountAdminQuery query)
        {
            if (!ModelState.IsValid)
                return View(query);

            query.UserId = _currentUser.Id;
            var validationErrors = await _dispatcher.Dispatch(query);
            if (validationErrors.Any())
            {
                validationErrors.ForEach(x => ModelState.AddModelError(x.Key, x.Value));
                return View(query);
            }

            TempData["Success"] = "Hesap başarıyla güncellendi";
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public IActionResult UpdatePassword()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> UpdatePassword([FromForm] UpdatePasswordAdminQuery query)
        {
            if (!ModelState.IsValid)
                return View(query);

            query.UserId = _currentUser.Id;
            var validationErrors = await _dispatcher.Dispatch(query);
            if (validationErrors.Any())
            {
                validationErrors.ForEach(x => ModelState.AddModelError(x.Key, x.Value));
                return View(query);
            }

            TempData["Success"] = "Parola başarıyla güncellendi";
            return RedirectToAction(nameof(UpdatePassword));
        }
    }
}