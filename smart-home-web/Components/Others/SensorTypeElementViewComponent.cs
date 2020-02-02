using Microsoft.AspNetCore.Mvc;
using smart_home_web.Models.SensorType;

namespace smart_home_web.Components.Others
{
    public class SensorTypeElementViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke(SensorTypeViewModel model) => View(model);
    }
}
