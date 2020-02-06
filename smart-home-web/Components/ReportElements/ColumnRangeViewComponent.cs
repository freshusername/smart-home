using AutoMapper;
using Infrastructure.Business.DTOs.ReportElements;
using Infrastructure.Business.Managers;
using Microsoft.AspNetCore.Mvc;
using smart_home_web.Models.ReportElements;
using System.Threading.Tasks;

namespace smart_home_web.Components.ReportElements
{
    public class ColumnrangeViewComponent : BaseViewComponent
    {
        private readonly IReportElementManager _reportElementManager;
        private readonly IMapper _mapper;
        public ColumnrangeViewComponent(IReportElementManager reportElementManager, IMapper mapper)
        {
            _reportElementManager = reportElementManager;
            _mapper = mapper;
        }

        public async Task<IViewComponentResult> InvokeAsync(int reportElementId)
        {
            ReportElementDto columnRangeDTO = await _reportElementManager.GetColumnRangeById(reportElementId);
            ReportElementViewModel model = _mapper.Map<ReportElementDto, ReportElementViewModel>(columnRangeDTO);
            return View(model);
        }
    }
}