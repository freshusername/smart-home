using Microsoft.AspNetCore.Mvc;
using smart_home_web.Models.SensorType;

namespace smart_home_web.Components.Others
{
    public class SensorTypeCreateViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke(CreateSensorTypeViewModel model) => View(model);
    }
}
