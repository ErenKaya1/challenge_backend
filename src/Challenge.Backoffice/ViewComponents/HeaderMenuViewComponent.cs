using Microsoft.AspNetCore.Mvc;

namespace Challenge.Backoffice.ViewComponents
{
    public class HeaderMenuViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View();
        }
    }
}