using Challenge.API.Controllers.Base;
using Microsoft.AspNetCore.Mvc;

namespace Challenge.API.Controllers
{
    public class TestController : BaseController
    {
        [HttpGet]
        public IActionResult Index()
        {
            return Ok(true);
        }
    }
}