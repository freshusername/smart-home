using AutoMapper;
using Domain.Core.Model;
using Domain.Core.Model.Enums;
using Infrastructure.Business.DTOs.Dashboard;
using Infrastructure.Business.DTOs.Icon;
using Infrastructure.Business.DTOs.Sensor;
using Infrastructure.Business.DTOs.SensorType;
using Infrastructure.Business.Infrastructure;
using Infrastructure.Business.Managers;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using smart_home_web.Models.Dashboard;
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
        private ISensorManager _sensorManager;

		public DashboardController(
			IMapper mapper, 
			IHostingEnvironment env, 
			IDashboardManager dashboardManager)
		{
			_mapper = mapper;
			_env = env;
			_dashboardManager = dashboardManager;
		}

		public async Task<IActionResult> Detail(int id)
		{
			var dashboard = await _dashboardManager.GetById(id);
			var result = _mapper.Map<DashboardDto, DashboardViewModel>(dashboard);

			return View(result);
		}

		public async Task<IActionResult> Index(int id)
		{
			var dashboard = await _dashboardManager.GetAll();
			var result = _mapper.Map<IEnumerable<DashboardDto>, IEnumerable<DashboardViewModel>>(dashboard);

			return View(new DashboardIndexViewModel
			{
				Dashboards = result
			}
			);
		}

        public async Task<IActionResult> Create(DashboardViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            var dashboard = _mapper.Map<DashboardViewModel, DashboardDto>(model);

            OperationDetails result = await _dashboardManager.Create(dashboard);
            if (result.Succeeded)
            {
                return RedirectToAction("Index");
            }
            else
            {
                ModelState.AddModelError(result.Property, result.Message);
                return RedirectToAction("Index");
            }
        }


        public IActionResult Delete(int id)
        {
            return View();
        } 

    }
}