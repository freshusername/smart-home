using AutoMapper;
using Domain.Core.Model;
using Domain.Core.Model.Enums;
using Infrastructure.Business.DTOs.Icon;
using Infrastructure.Business.DTOs.Sensor;
using Infrastructure.Business.DTOs.SensorType;
using Infrastructure.Business.Managers;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using smart_home_web.Models.SensorType;
using smart_home_web.Models.SensorViewModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace smart_home_web.Controllers
{
    [Route("[controller]/[action]")]
    public class DashboardController : Controller
    {
        private readonly IMapper _mapper;
        private IHostingEnvironment _env;
        private IDashboardManager _dashboardManager;

		public DashboardController(
			IMapper mapper, 
			IHostingEnvironment env, 
			IDashboardManager dashboardManager)
		{
			_mapper = mapper;
			_env = env;
			_dashboardManager = dashboardManager;
		}

        [HttpGet]
        public IActionResult GetSensorsByReportElementType(ReportElementType type, int dashboardId)
        {
            List<int> list = new List<int>
            {
                5,4,3,2,1
            };
            return Ok(list);
        }
    }
}
