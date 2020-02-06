using Microsoft.AspNetCore.Mvc;
using smart_home_web.Models.SensorViewModel;

namespace smart_home_web.Components.Others
{
    public class SensorCreateViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke(CreateSensorViewModel model) => View(model);
    }
}
