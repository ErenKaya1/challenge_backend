using Microsoft.AspNetCore.Mvc;
using Challenge.Backoffice.Controllers.Base;

namespace Challenge.Backoffice.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
