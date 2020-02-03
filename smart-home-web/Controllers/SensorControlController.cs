using AutoMapper;
using Infrastructure.Business.DTOs;
using Infrastructure.Business.DTOs.Sensor;
using Infrastructure.Business.Interfaces;
using Infrastructure.Business.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using smart_home_web.Models;
using smart_home_web.Models.ControlSensor;
using smart_home_web.Models.SensorViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace smart_home_web.Controllers
{
    [Route("[controller]/[action]")]
    public class SensorControlController : Controller
    {
        private readonly ISensorControlManager _sensorControlManager;
        private readonly ISensorManager _sensorManager;
        private readonly IMapper _mapper;

        public SensorControlController(ISensorControlManager sensorControlManager ,ISensorManager sensorManager , IMapper mapper)
        {
            _sensorControlManager = sensorControlManager;
            _sensorManager = sensorManager;
            _mapper = mapper;
        }

        [Authorize]
        [HttpGet]
        public IActionResult Index()
        {
            var data = _sensorControlManager.GetSensorControls();
            var model = _mapper.Map<List<SensorControlDto>, List<SensorControlViewModel>>(data);
            return View(model);
        }
       
        [Authorize]
        [HttpPost]
        public IActionResult Change(int id, bool isActive)
        {
            var result = _sensorControlManager.UpdateById(id,isActive);
            if (!result.Succeeded) return BadRequest();
              
            return Ok();
        }

        [Authorize]
        [HttpPost]
        public IActionResult Delete(int id)
        {
            var result = _sensorControlManager.Delete(id);
            if (!result.Succeeded) return BadRequest();

            return Ok();
        }

        [Authorize]
        [HttpGet]
        public IActionResult Edit(int id)
        {   
            var sensorControl = _sensorControlManager.GetById(id);
             var controlSensors = _sensorManager.GetControlSensors();
            var sensors = _sensorManager.GetSensorsToControl();

             if(sensorControl == null) return View("Error");

            var model = _mapper.Map<SensorControlDto,EditSensorControlViewModel>(sensorControl);
                                  
            ViewBag.modelControlSensors = new SelectList(_mapper.Map<List<SensorDto>, List<SensorViewModel>>(controlSensors), "Id", "Name");
             ViewBag.modelSensors = new SelectList(_mapper.Map<List<SensorDto>, List<SensorViewModel>>(sensors), "Id", "Name");

            return View(model);                                   
        }

        [Authorize]
        [HttpPost]
        public IActionResult Edit(EditSensorControlViewModel model)
        {
            if (ModelState.IsValid)
            {
                var sensorControl = _mapper.Map<EditSensorControlViewModel, SensorControlDto>(model);
                 var result = _sensorControlManager.Update(sensorControl);
                if (!result.Succeeded) ModelState.AddModelError("", result.Message);

                return RedirectToAction("Index");
            }

            return View(model);
        }

        [Authorize]
        [HttpGet]
        public IActionResult Add()
        {
           
            var controlSensors = _sensorManager.GetControlSensors();
             var sensors = _sensorManager.GetSensorsToControl();
            
            ViewBag.modelControlSensors = new SelectList(_mapper.Map<List<SensorDto>, List<SensorViewModel>>(controlSensors), "Id", "Name");
            ViewBag.modelSensors = new SelectList(_mapper.Map<List<SensorDto>, List<SensorViewModel>>(sensors), "Id", "Name");

            return View();
        }

        [Authorize]
        [HttpPost]
        public IActionResult Add(AddSensorControlViewModel model)
        {
            if (ModelState.IsValid)
            {
                var sensorControl = _mapper.Map<AddSensorControlViewModel, SensorControlDto>(model);
                var result = _sensorControlManager.Add(sensorControl);
                if (!result.Succeeded) ModelState.AddModelError("", result.Message);

                return RedirectToAction("Index");
            }

            return View(model);
        }
    
    }
}
