using AutoMapper;
using Domain.Core.Model;
using Infrastructure.Business.DTOs.Icon;
using Infrastructure.Business.DTOs.Sensor;
using Infrastructure.Business.Managers;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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
        private IHostingEnvironment _env;

        public SensorController(IMapper mapper, ISensorManager sensorManager, IIconManager iconManager, IHostingEnvironment env)
        {
            _sensorManager = sensorManager;
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
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> AddSensor(CreateSensorViewModel sensor)
        {

            if (sensor.IconFile != null)
            {
                
                SensorDto sensorDto = _mapper.Map<CreateSensorViewModel, SensorDto>(sensor);
                sensorDto.IconId = await _iconManager.CreateAndGetIconId(sensor.IconFile);
                
                await _sensorManager.Create(sensorDto);
            }
            return RedirectToAction("Index", "Sensor");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
