using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace smart_home_web.Components
{
    public class DashboardViewComponent : ViewComponent
    {
        public async Task<IViewComponentResult> Invoke()
        {
            return View();
        }
    }
}
