using Microsoft.AspNetCore.Mvc;
using smart_home_web.Models.History;

namespace smart_home_web.Components.Others
{
    public class HistoryViewComponent: ViewComponent
    {
        public IViewComponentResult Invoke(AllHistoriesViewModel model) => View(model);
    }
}
