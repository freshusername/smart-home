using AutoMapper;
using Infrastructure.Business.DTOs.ReportElements;
using Infrastructure.Business.Managers;
using Microsoft.AspNetCore.Mvc;
using smart_home_web.Models.ReportElements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace smart_home_web.Components
{
    public class HeatmapViewComponent : BaseViewComponent
    {
        private readonly IReportElementManager _reportElementManager;
        private readonly IMapper _mapper;
        public HeatmapViewComponent(IReportElementManager reportElementManager, IMapper mapper)
        {
            _reportElementManager = reportElementManager;
            _mapper = mapper;
        }
        public async Task<IViewComponentResult> InvokeAsync(int heatmapId)
        {
            HeatmapDto heatmapDto = await _reportElementManager.GetHeatmapById(heatmapId);
            HeatmapViewModel result = _mapper.Map<HeatmapDto, HeatmapViewModel>(heatmapDto);
            return View("Heatmap", result);
        }
    }
}
