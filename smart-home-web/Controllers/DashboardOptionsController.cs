using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Infrastructure.Business.DTOs.ReportElements;
using Infrastructure.Business.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using Domain.Core.Model;
using Infrastructure.Business.Managers;

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

		[HttpPost]
		public async Task UpdateOptions(IEnumerable<ReportElement> options)
		{
			foreach (ReportElement reportElement in options)
			{
				await _reportElementManager.Update(reportElement);
			}
		}
	}
}