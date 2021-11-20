using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Challenge.Backoffice.Controllers.Base
{
    [Authorize(Roles = "Admin,Moderator")]
    public class BaseController : Controller
    {

    }
}