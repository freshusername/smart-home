using AutoMapper;
using Infrastructure.Business.DTOs.ReportElements;
using Infrastructure.Business.Interfaces;
using Infrastructure.Business.Managers;
using Microsoft.AspNetCore.Mvc;
using smart_home_web.Components.ReportElements;
using smart_home_web.Models.ReportElements;
using System.Threading.Tasks;

namespace smart_home_web.Components
{
    public class BoolHeatmapViewComponent : BaseViewComponent
    {
        private readonly IReportElementManager _reportElementManager;
        private readonly IMapper _mapper;
        public BoolHeatmapViewComponent(IReportElementManager reportElementManager, IMapper mapper)
        {
            _reportElementManager = reportElementManager;
            _mapper = mapper;
        }
        public async Task<IViewComponentResult> InvokeAsync(int reportElementId)
        {
            BoolHeatmapDto boolHeatmapDto = await _reportElementManager.GetBoolHeatmapById(reportElementId);
            BoolHeatmapViewModel result = _mapper.Map<BoolHeatmapDto, BoolHeatmapViewModel>(boolHeatmapDto);

            return View("BoolHeatmap", result);
        }
    }
}
