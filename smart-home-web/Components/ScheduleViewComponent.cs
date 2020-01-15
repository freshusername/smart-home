using AutoMapper;
using Domain.Core.Model.Enums;
using Infrastructure.Business.DTOs.ReportElements;
using Infrastructure.Business.DTOs.SensorType;
using Infrastructure.Business.Managers;
using Microsoft.AspNetCore.Mvc;
using smart_home_web.Models;
using smart_home_web.Models.ReportElements;
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

        public async Task<IViewComponentResult> InvokeAsync(int id , ReportElementHours hours)
        {
            var data = await _reportElementManager.GetDataForSchedule(id , hours);
             if (data == null) ModelState.AddModelError("" , " Theare is no measurement data");

            var result = _mapper.Map<ReportElementDto,ReportElementViewModel>(data);
            
            return View(result);           
        }
    }
}