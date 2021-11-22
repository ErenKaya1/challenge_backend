using System.Threading.Tasks;
using Challenge.Application.Business.Policies.Commands;
using Challenge.Application.Business.Policies.Queries;
using Challenge.Backoffice.Controllers.Base;
using Challenge.Backoffice.Models.Policy;
using Challenge.Common;
using Microsoft.AspNetCore.Mvc;

namespace Challenge.Backoffice.Controllers
{
    public class PolicyController : BaseController
    {
        private readonly Dispatcher _dispatcher;

        public PolicyController(Dispatcher dispatcher)
        {
            _dispatcher = dispatcher;
        }

        [HttpGet]
        public async Task<IActionResult> Index([FromQuery] GetPoliciesQuery query)
        {
            var response = new PolicyIndexVM
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
                var policy = await _dispatcher.Dispatch(new GetPolicyQuery { Id = id });
                return View(new AddUpdatePolicyCommand { Policy = policy });
            }

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddOrUpdate([FromForm] AddUpdatePolicyCommand command)
        {
            if (!ModelState.IsValid)
                return View(command);

            await _dispatcher.Dispatch(command);
            TempData["Success"] = "Sözleşme başarıyla kaydedildi.";

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Delete([FromRoute] string id)
        {
            if (string.IsNullOrWhiteSpace(id))
                return NotFound();

            var policy = await _dispatcher.Dispatch(new GetPolicyQuery { Id = id });
            await _dispatcher.Dispatch(new DeletePolicyCommand { Policy = policy });
            TempData["Success"] = "Sözleşme başarıyla silindi.";

            return RedirectToAction(nameof(Index));
        }
    }
}