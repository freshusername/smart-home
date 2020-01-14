using AutoMapper;
using Domain.Core.Model;
using Infrastructure.Business.DTOs.DashboardOptions;
using Infrastructure.Business.DTOs.Icon;
using Infrastructure.Business.DTOs.Sensor;
using Infrastructure.Business.DTOs.SensorType;
using Infrastructure.Business.Managers;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using smart_home_web.Models.DashboardOptions;
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
    public class DashboardOptionsController : Controller
    {
        private readonly IMapper _mapper;
        private IHostingEnvironment _env;
		private IDashboardOptionsManager _dashboardOptionsManager;

		public DashboardOptionsController(IMapper mapper, IHostingEnvironment env, IDashboardOptionsManager dashboardOptionsManager)
		{
			_mapper = mapper;
			_env = env;
			_dashboardOptionsManager = dashboardOptionsManager;
		}

		//[HttpPost]
  //      public async Task<ActionResult> AddDashboardOptions(DashboardOptionsViewModel dashboardOptionsViewModel)
  //      {
		//	DashboardOptionsDto dashboardOptionsDto = _mapper.Map<DashboardOptionsViewModel, DashboardOptionsDto>(dashboardOptionsViewModel);

  //          await _dashboardOptionsManager.
  //          return RedirectToAction("Index", "Sensor");
  //      }
    }
}
