using Microsoft.AspNetCore.Mvc;

namespace Challenge.Backoffice.ViewComponents
{
    public class FooterViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View();
        }
    }
}