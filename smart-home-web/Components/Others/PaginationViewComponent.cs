using Infrastructure.Business.DTOs;
using smart_home_web.Models;
using Microsoft.AspNetCore.Mvc;

namespace smart_home_web.Components.Others
{
    public class PaginationViewComponent:ViewComponent
    {
        public IViewComponentResult Invoke(string controller, string action, FilterDto filterDTO)
        {
            PaginationViewModel model = new PaginationViewModel
            {
                controller = controller,
                action = action,
                filterDto = filterDTO
            };
            return View(model);
        }
    }
}
