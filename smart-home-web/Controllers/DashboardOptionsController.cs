using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Infrastructure.Business.DTOs.ReportElements;
using Infrastructure.Business.Managers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using Domain.Core.Model;

namespace smart_home_web.Controllers
{
    public class DashboardOptionsController : ControllerBase
    {
        private readonly IReportElementManager _reportElementManager;
        private readonly IDashboardManager _dashboardManager;
        private readonly IMapper _mapper;

        public DashboardOptionsController(IReportElementManager reportElementManager, IMapper mapper, IDashboardManager dashboardManager)
        {
			_dashboardManager = dashboardManager;
            _reportElementManager = reportElementManager;
            _mapper = mapper;
        }

        //[HttpGet("GetGauge/{id}")]
        //public async Task<IActionResult> Get(int id)
        //{
        //    GaugeDto gaugeDto = await _reportElementManager.GetGaugeById(id);
        //    GaugeUpdateViewModel result = _mapper.Map<GaugeDto, GaugeUpdateViewModel>(gaugeDto);

        //    return Ok(result);
        //}

		[HttpPost]
		public async Task UpdateOptions(IEnumerable<ReportElement> options)
		{
			foreach(ReportElement reportElement in options)
			{ 
				await _reportElementManager.Update(reportElement);
			}
		}
	}
}