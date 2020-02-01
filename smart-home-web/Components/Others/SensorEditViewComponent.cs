using Microsoft.AspNetCore.Mvc;
using smart_home_web.Models.SensorViewModel;

namespace smart_home_web.Components.Others
{
    public class SensorEditViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke(EditSensorViewModel model) => View(model);
    }
}
