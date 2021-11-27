using System.Threading.Tasks;
using Challenge.API.Controllers.Base;
using Challenge.Application.Business.Policies.Entities;
using Challenge.Application.Business.Policies.Queries;
using Challenge.Common;
using Challenge.Core.Response;
using Microsoft.AspNetCore.Mvc;
using static Challenge.Core.Enums.Enums;

namespace Challenge.API.Controllers
{
    public class PolicyController : AuthorizedController
    {
        private readonly Dispatcher _dispatcher;

        public PolicyController(Dispatcher dispatcher)
        {
            _dispatcher = dispatcher;
        }

        [HttpGet]
        public async Task<IActionResult> KVKK()
        {
            var policy = await _dispatcher.Dispatch(new GetPolicyByTypeQuery { PolicyType = PolicyType.KVKK });
            var response = new BaseResponse<Policy> { Data = policy };

            return Ok(response);
        }
    }
}