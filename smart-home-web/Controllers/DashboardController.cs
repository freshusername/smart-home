using AutoMapper;
using Domain.Core.Model;
using Domain.Core.Model.Enums;
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
    public class DashboardController : Controller
    {
        private readonly IMapper _mapper;
        private IHostingEnvironment _env;
        private IDashboardOptionsManager _dashboardOptionsManager;
        private IDashboardManager _dashboardManager;
        private ISensorManager _sensorManager;

        public DashboardController(
            IMapper mapper,
            IHostingEnvironment env,
            IDashboardOptionsManager dashboardOptionsManager,
            IDashboardManager dashboardManager,
            ISensorManager sensorManager)
        {
            _mapper = mapper;
            _env = env;
            _dashboardOptionsManager = dashboardOptionsManager;
            _dashboardManager = dashboardManager;
            _sensorManager = sensorManager;
        }

        [HttpPost]
        public async Task<ActionResult> AddDashboardOptions(DashboardOptionsViewModel dashboardOptionsViewModel)
        {
            DashboardOptionsDto dashboardOptionsDto = _mapper.Map<DashboardOptionsViewModel, DashboardOptionsDto>(dashboardOptionsViewModel);

            //_dashboardOptionsManager.Create(dashboardOptionsDto);

            return RedirectToAction("Index", "Sensor");
        }

    }
}
