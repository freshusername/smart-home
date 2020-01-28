using AutoMapper;
using Infrastructure.Business.DTOs.History;
using Infrastructure.Business.DTOs.ReportElements;
using Infrastructure.Business.Managers;
using Microsoft.AspNetCore.Mvc;
using smart_home_web.Models.History;
using smart_home_web.Models.ReportElements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace smart_home_web.Components
{
    public class StatusReportViewComponent: BaseViewComponent
    {
        private readonly IReportElementManager _reportElementManager;
        private readonly IMapper _mapper;
        public StatusReportViewComponent(IReportElementManager reportElementManager, IMapper mapper)
        {
            _reportElementManager = reportElementManager;
            _mapper = mapper;
        }
        public async Task<IViewComponentResult> Invoke(int reportElementId)
        {
            ReportElementDto element = await _reportElementManager.GetStatusReport(reportElementId);
            ReportElementViewModel model = _mapper.Map<ReportElementDto, ReportElementViewModel>(element);
            return View();
        }
    }
}
