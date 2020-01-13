using AutoMapper;
using Infrastructure.Business.DTOs.ReportElements;
using Infrastructure.Business.Managers;
using Microsoft.AspNetCore.Mvc;
using smart_home_web.Models.ReportElements;
using System.Threading.Tasks;

namespace smart_home_web.Components
{
    public class WordCloudViewComponent : BaseViewComponent
    {
        private readonly IReportElementManager _reportElementManager;
        private readonly IMapper _mapper;
        public WordCloudViewComponent(IReportElementManager reportElementManager, IMapper mapper)
        {
            _reportElementManager = reportElementManager;
            _mapper = mapper;
        }

        public async Task<IViewComponentResult> InvokeAsync(int reportElementId)
        {
            ReportElementDTO wordCloud = await _reportElementManager.GetWordCloudById(reportElementId);
            ReportElementViewModel result = _mapper.Map<ReportElementDTO, ReportElementViewModel>(wordCloud);
            return View("WordCloud", result);
        }
    }
}
