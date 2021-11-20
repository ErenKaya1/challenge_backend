using Microsoft.AspNetCore.Mvc;

namespace Challenge.Backoffice.ViewComponents
{
    public class SidebarMenuViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View();
        }
    }
}