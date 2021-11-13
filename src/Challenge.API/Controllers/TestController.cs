using Microsoft.AspNetCore.Mvc;

namespace Challenge.API.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class TestController : ControllerBase
    {
        [HttpGet]
        public IActionResult Index()
        {
            return Ok(true);
        }
    }
}