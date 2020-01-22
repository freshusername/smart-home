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
    public class SensorController : Controller
    {
        private readonly IMapper _mapper;
        private readonly ISensorManager _sensorManager;
        private readonly IIconManager _iconManager;
        private readonly ISensorTypeManager _sensorTypeManager;
        private IHostingEnvironment _env;

        public SensorController(IMapper mapper,
            ISensorManager sensorManager,
            IIconManager iconManager,
            ISensorTypeManager sensorTypeManager,
            IHostingEnvironment env)
        {
            _sensorManager = sensorManager;
            _sensorTypeManager = sensorTypeManager;
            _iconManager = iconManager;
            _mapper = mapper;
            _env = env;
        }

        [HttpGet]
        public async Task<ActionResult> Index()
        {
            var sensors = _mapper.Map<IEnumerable<SensorViewModel>>(await _sensorManager.GetAllSensorsAsync());
            return View(sensors);
        }

        [HttpGet]
        public IActionResult Create()
        {
            IEnumerable<SensorTypeDto> sensorTypeDtos = _sensorTypeManager.GetAllSensorTypesAsync().Result;
            var sensorTypes = _mapper.Map<IEnumerable<SensorTypeDto>, IEnumerable<SensorTypeViewModel>>(sensorTypeDtos);
            ViewBag.sensorTypes = sensorTypes;
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> AddSensor(CreateSensorViewModel sensor)
        {
            SensorDto sensorDto = _mapper.Map<CreateSensorViewModel, SensorDto>(sensor);
            if (sensor.IconFile != null)
            {
                sensorDto.IconId = await _iconManager.CreateAndGetIconId(sensor.IconFile);
            }

            await _sensorManager.Create(sensorDto);
            return RedirectToAction("Index", "Sensor");
        }

        public async Task<ActionResult> Update(int sensorId)
        {
            var sensorDto = await _sensorManager.GetSensorByIdAsync(sensorId);
            EditSensorViewModel sensorViewModel = _mapper.Map<SensorDto, EditSensorViewModel>(sensorDto);
            return View("Update", sensorViewModel);
        }
        public async Task<ActionResult> Delete(int sensorId)
        {
            var sensorDto = await _sensorManager.GetSensorByIdAsync(sensorId);
            SensorViewModel sensorViewModel = _mapper.Map<SensorDto, SensorViewModel>(sensorDto);
            return View("Delete", sensorViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Update(EditSensorViewModel sensorViewModel)
        {
            SensorDto sensorDto = _mapper.Map<EditSensorViewModel, SensorDto>(sensorViewModel);
            if (sensorViewModel.IconFile != null)
            {
                sensorDto.IconId = await _iconManager.CreateAndGetIconId(sensorViewModel.IconFile);
            }
            try
            {
                _sensorManager.Update(sensorDto);

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View("Update");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(SensorViewModel sensorViewModel)
        {
            SensorDto sensorDto = _mapper.Map<SensorViewModel, SensorDto>(sensorViewModel);

            try
            {
                _sensorManager.Delete(sensorDto);

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View("Delete");
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetSensorsByReportElementType(ReportElementType type, int dashboardId)
        {
            var res = await _sensorManager.GetSensorsByReportElementType(type, dashboardId);
            return Ok(res.ToList());
        }
    }
}
