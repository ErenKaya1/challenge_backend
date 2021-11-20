using System.Linq;
using Microsoft.AspNetCore.Http;

namespace Challenge.Application.Services.CurrentUser
{
    public class CurrentUserService : ICurrentUserService
    {
        private readonly IHttpContextAccessor _accessor;

        public CurrentUserService(IHttpContextAccessor accessor)
        {
            _accessor = accessor;
        }

        public string UserId
        {
            get
            {
                return _accessor.HttpContext.User.Claims.FirstOrDefault(x => x.Type == "userId")?.Value ?? _accessor.HttpContext.User.FindFirst("sub")?.Value;
            }
        }
    }
}