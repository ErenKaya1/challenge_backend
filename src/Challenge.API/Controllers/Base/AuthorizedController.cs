using Microsoft.AspNetCore.Authorization;

namespace Challenge.API.Controllers.Base
{
    [Authorize]
    public class AuthorizedController : BaseController
    {

    }
}