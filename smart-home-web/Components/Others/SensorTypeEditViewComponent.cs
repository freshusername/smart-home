using Microsoft.AspNetCore.Mvc;
using smart_home_web.Models.SensorType;

namespace smart_home_web.Components.Others
{
    public class SensorTypeEditViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke(EditSensorTypeViewModel model) => View(model);
    }
}
