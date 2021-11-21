using System.Linq;
using System.Threading.Tasks;
using Challenge.Application.Business.Users.Commands;
using Challenge.Application.Business.Users.Queries;
using Challenge.Application.Business.Users.Request;
using Challenge.Backoffice.Controllers.Base;
using Challenge.Backoffice.Models.BackofficeUser;
using Challenge.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Challenge.Backoffice.Controllers
{
    [Authorize(Roles = "Admin")]
    public class BackofficeUserController : BaseController
    {
        private readonly Dispatcher _dispatcher;

        public BackofficeUserController(Dispatcher dispatcher)
        {
            _dispatcher = dispatcher;
        }

        [HttpGet]
        public async Task<IActionResult> Index([FromQuery] GetBackofficeUsersQuery query)
        {
            var response = new BackofficeUserIndexVM
            {
                Query = query,
                Result = await _dispatcher.Dispatch(query)
            };

            return View(response);
        }

        [HttpGet]
        public async Task<IActionResult> AddOrUpdate([FromRoute] string id)
        {
            if (!string.IsNullOrWhiteSpace(id))
            {
                var user = await _dispatcher.Dispatch(new GetUserQuery { Id = id });
                return View(new AddUpdateBackofficeUserRequest
                {
                    Id = user.Id,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Email = user.Email,
                    PhoneNumber = user.PhoneNumber,
                    Role = user.Role,
                });
            }

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddOrUpdate([FromForm] AddUpdateBackofficeUserRequest request)
        {
            if (!ModelState.IsValid)
                return View(request);

            request.RegisterIpAddress = HttpContext.Connection.RemoteIpAddress.MapToIPv4().ToString();
            var validationErrors = await _dispatcher.Dispatch(new AddUpdateBackofficeUserQuery { Request = request });
            if (validationErrors.Any())
            {
                validationErrors.ForEach(x => ModelState.AddModelError(x.Key, x.Value));
                return View(request);
            }
            else
            {
                TempData["Success"] = "Kullanıcı başarıyla kaydedildi.";
                return RedirectToAction(nameof(Index));
            }
        }

        [HttpGet]
        public async Task<IActionResult> Delete([FromRoute] string id, string returnUrl)
        {
            if (string.IsNullOrWhiteSpace(id))
                return NotFound();

            var user = await _dispatcher.Dispatch(new GetUserQuery { Id = id });
            await _dispatcher.Dispatch(new DeleteUserCommand { User = user });
            TempData["Success"] = "Kullanıcı başarıyla silindi.";

            if (string.IsNullOrWhiteSpace(returnUrl))
                return RedirectToAction(nameof(Index));

            return LocalRedirect(returnUrl);
        }
    }
}