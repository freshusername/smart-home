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

		public async Task<IActionResult> Index()
		{
			var dashboard = await _dashboardManager.GetAll();
			var result = _mapper.Map<IEnumerable<DashboardDto>, IEnumerable<DashboardViewModel>>(dashboard);

			return View(new DashboardIndexViewModel
			{
				Dashboards = result.Reverse()
			}
			);
		}
        
        [HttpPost]
        public async Task<IActionResult> Create(string name)
        {
            DashboardDto dashboardDto = new DashboardDto()
            {
                Name = name
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

        [HttpPost]
        public async Task<IActionResult> Edit(int id, string name)
        {
            DashboardDto dashboardDto = new DashboardDto()
            {
                Name = name,
                Id=id
            };

            await _dashboardManager.Update(id, name);

            return Ok();
        }

        public async Task<IActionResult> Delete(int id)
        {
            await _dashboardManager.DeleteById(id);
            return Ok();

        } 

        [HttpGet]
        public IActionResult Create()
        {
            return PartialView();
        }


    }
}