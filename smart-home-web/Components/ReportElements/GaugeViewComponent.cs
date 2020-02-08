using AutoMapper;
using Infrastructure.Business.DTOs.ReportElements;
using Infrastructure.Business.Interfaces;
using Microsoft.AspNetCore.Mvc;
using smart_home_web.Models.ReportElements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace smart_home_web.Components.ReportElements
{
    public class GaugeViewComponent : BaseViewComponent
    {
        private readonly IReportElementManager _reportElementManager;
        private readonly IMapper _mapper;
        public GaugeViewComponent(IReportElementManager reportElementManager, IMapper mapper)
        {
            _reportElementManager = reportElementManager;
            _mapper = mapper;
        }
        public async Task<IViewComponentResult> InvokeAsync(int reportElementId)
        {
            GaugeDto gaugeDto = await _reportElementManager.GetGaugeById(reportElementId);
            GaugeViewModel result = _mapper.Map<GaugeDto, GaugeViewModel>(gaugeDto);
            return View(result);
        }
    }
}
