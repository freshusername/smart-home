using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using smart_home_web.Models.Dashboard;

namespace smart_home_web.Components
{
    public class DashboardCardViewComponent : BaseViewComponent
    {
        public DashboardCardViewComponent(IMapper mapper)
        {

        }

        public IViewComponentResult Invoke(DashboardViewModel model)
        {
            return View(model);
        }
    }
}
