using Microsoft.AspNetCore.Mvc;
using smart_home_web.Models.Dashboard;

namespace smart_home_web.Components.Others
{
    public class DashboardEditViewComponent: ViewComponent
    {
        public IViewComponentResult Invoke(EditDashboardViewModel model) => View(model);
    }
}
