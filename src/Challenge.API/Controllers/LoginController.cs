using System.Threading.Tasks;
using Challenge.API.Controllers.Base;
using Challenge.Application.Business.Users.Queries;
using Challenge.Common;
using Challenge.Core.Response;
using Challenge.Core.Settings;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace Challenge.API.Controllers
{
    public class LoginController : BaseController
    {
        private readonly Dispatcher _dispatcher;

        public LoginController(Dispatcher dispatcher, IOptions<MongoDBSettings> mongoSettings)
        {
            System.Console.WriteLine(mongoSettings.Value.ConnectionString);
            _dispatcher = dispatcher;
        }

        [HttpPost]
        public async Task<IActionResult> SignUp([FromBody] SignUpQuery query)
        {
            var response = new BaseResponse<bool>();
            query.IpAddress = HttpContext.Connection.RemoteIpAddress.MapToIPv4().ToString();
            var user = await _dispatcher.Dispatch(query);

            response.SetMessage("Başarıyla kayıt olundu.");

            return Ok(response);
        }
    }
}