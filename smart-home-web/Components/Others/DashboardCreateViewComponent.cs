using Microsoft.AspNetCore.Mvc;
using smart_home_web.Models.Dashboard;


namespace smart_home_web.Components.Others
{
    public class DashboardCreateViewComponent: ViewComponent
    {
        public IViewComponentResult Invoke(CreateDashboardViewModel model) => View(model);
    }
}
