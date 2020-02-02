using Microsoft.AspNetCore.Mvc;
using smart_home_web.Models.SensorViewModel;

namespace smart_home_web.Components.Others
{
    public class SensorElementViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke(SensorViewModel model) => View(model);
    }
}
