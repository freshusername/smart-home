
using Infrastructure.Business.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using smart_home_web.Models;
using Microsoft.AspNetCore.Mvc;

namespace smart_home_web.Components
{
    public class PaginationViewComponent:ViewComponent
    {
        public IViewComponentResult Invoke(string controller,string action,FilterDto filterDTO)
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
