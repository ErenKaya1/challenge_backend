using Microsoft.AspNetCore.Mvc;

namespace Challenge.Backoffice.ViewComponents
{
    public class AlertViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View();
        }
    }
}