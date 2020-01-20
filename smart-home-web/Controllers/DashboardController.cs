using AutoMapper;
using Domain.Core.Model;
using Domain.Core.Model.Enums;
using Infrastructure.Business.DTOs.Dashboard;
using Infrastructure.Business.DTOs.Icon;
using Infrastructure.Business.DTOs.ReportElements;
using Infrastructure.Business.DTOs.Sensor;
using Infrastructure.Business.DTOs.SensorType;
using Infrastructure.Business.Managers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using smart_home_web.Models;
using smart_home_web.Models.Dashboard;
using smart_home_web.Models.SensorType;
using smart_home_web.Models.SensorViewModel;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
		private ISensorManager _sensorManager;
		private UserManager<AppUser> _userManager;

		public DashboardController(
			IMapper mapper,
			IHostingEnvironment env,
			IDashboardManager dashboardManager,
			UserManager<AppUser> userManager)
		{
			_mapper = mapper;
			_env = env;
			_dashboardManager = dashboardManager;
			_userManager = userManager;
		}

		[Authorize]
		public async Task<IActionResult> Detail(int id)
		{
			var userId = _userManager.GetUserId(User);
			var dashboard = await _dashboardManager.GetById(id);
			if (userId != dashboard.AppUserId)
			{
				return StatusCode(403);
			}
			var result = _mapper.Map<DashboardDto, DashboardViewModel>(dashboard);

			return View(result);
		}

		[Authorize]
		public async Task<IActionResult> Index(int id)
		{
			var userId = _userManager.GetUserId(User);
			var dashboardDtos = await _dashboardManager.GetByUserId(userId);
			var result = _mapper.Map<IEnumerable<DashboardDto>, IEnumerable<DashboardViewModel>>(dashboardDtos);

			return View(new DashboardIndexViewModel
			{
				Dashboards = result
			});
		}
	}
}