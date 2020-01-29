using Microsoft.AspNetCore.Mvc;
using smart_home_web.Models.Dashboard;

namespace smart_home_web.Components.Others
{
    public class DashboardViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke(DashboardViewModel model) => View(model);
    }
}
