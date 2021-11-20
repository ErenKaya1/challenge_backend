using Challenge.Backoffice.Controllers.Base;
using Microsoft.AspNetCore.Mvc;

namespace Challenge.Backoffice.Controllers
{
    public class HomeController : BaseController
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
