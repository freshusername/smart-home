using AutoMapper;
using Infrastructure.Business.DTOs.ReportElements;
using Infrastructure.Business.Managers;
using Microsoft.AspNetCore.Mvc;
using smart_home_web.Models.WordCloud;
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
            WordCloudDTO wordCloud = await _reportElementManager.GetWordCloudById(reportElementId);
            WordCloudViewModel result = _mapper.Map<WordCloudDTO, WordCloudViewModel>(wordCloud);
            return View("WordCloud", result);
        }
    }
}
