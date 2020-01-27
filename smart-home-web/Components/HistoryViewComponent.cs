using Microsoft.AspNetCore.Mvc;
using smart_home_web.Models.History;

namespace smart_home_web.Components
{
    public class HistoryViewComponent: ViewComponent
    {
        public IViewComponentResult Invoke(AllHistoriesViewModel model)
        {
            return View(model);
        }
    }
}
