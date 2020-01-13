using Infrastructure.Business.Managers;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace smart_home_web.Components
{
    public class ScheduleViewComponent : ViewComponent
    {
        private readonly IReportElementManager _reportElementManager;

        public ScheduleViewComponent(IReportElementManager reportElementManager)
        {
            _reportElementManager = reportElementManager;
        }

        public IViewComponentResult Invoke(int id)
        {
          var  data =  _reportElementManager.GetDataForSchedule(id);

           return View(data);
        }
    }
}