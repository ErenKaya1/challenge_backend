using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Challenge.Backoffice.Controllers.Base
{
    [Authorize("Admin,Moderator")]
    public class BaseController : Controller
    {
        
    }
}