using Microsoft.AspNetCore.Mvc;

namespace Challenge.Presentation.Controllers;

public class HomeController : Controller
{
    public IActionResult Index()
    {
        return View();
    }
}
