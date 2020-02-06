using Microsoft.AspNetCore.Mvc;

namespace smart_home_web.Components.Others
{
    public class DashboardCreateViewComponent: ViewComponent
    {
        public IViewComponentResult Invoke(string userid)
        {
            ViewBag.UserID = userid;
            return View();
        }
    }
}
