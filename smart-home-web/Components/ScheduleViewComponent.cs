using AutoMapper;
using Infrastructure.Business.DTOs.ReportElements;
using Infrastructure.Business.DTOs.SensorType;
using Infrastructure.Business.Managers;
using Microsoft.AspNetCore.Mvc;
using smart_home_web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace smart_home_web.Components
{
    public class ScheduleViewComponent : ViewComponent
    {
        private readonly IReportElementManager _reportElementManager;
        private readonly IMapper _mapper;

        public ScheduleViewComponent(IReportElementManager reportElementManager,IMapper mapper)
        {
            _reportElementManager = reportElementManager;
            _mapper = mapper;
        }

        public async Task<IViewComponentResult> InvokeAsync(int id , int days = 30)
        {
          var  data = await _reportElementManager.GetDataForSchedule(id , days);
            var result = _mapper.Map<ReportElementDto,ReportElementViewModel>(data);
              if(result != null) return View(result);

            return View();           
        }
    }
}