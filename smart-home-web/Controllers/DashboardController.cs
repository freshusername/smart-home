using AutoMapper;
using Domain.Core.Model;
using Infrastructure.Business.DTOs.Dashboard;
using Infrastructure.Business.Infrastructure;
using Infrastructure.Business.Managers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using smart_home_web.Models.Dashboard;
using System.Collections.Generic;
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

		[Authorize]
		[HttpPost]
		public async Task<IActionResult> Create(string name)
		{
			DashboardDto dashboardDto = new DashboardDto()
			{
				Name = name,
				AppUserId = _userManager.GetUserId(User)

			};

			OperationDetails result = await _dashboardManager.Create(dashboardDto);
			if (result.Succeeded)
			{
				var dashboardDtos = await _dashboardManager.GetAll();
				var dashboard = _mapper.Map<DashboardDto, DashboardViewModel>(dashboardDtos.Last());
				return ViewComponent("Dashboard", new { model = dashboard });
			}
			else
			{
				ModelState.AddModelError(result.Property, result.Message);
				return NotFound();
			}
		}

		[Authorize]
		[HttpPost]
		public async Task<IActionResult> Edit(int id, string name)
		{
			await _dashboardManager.Update(id, name);
			return Ok();
		}

		public async Task<IActionResult> Delete(int id)
		{
			await _dashboardManager.DeleteById(id);
			return Ok();

		}
	}
}