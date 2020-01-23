using AutoMapper;
using Infrastructure.Business.DTOs;
using Infrastructure.Business.DTOs.Sensor;
using Infrastructure.Business.Interfaces;
using Infrastructure.Business.Managers;
using Microsoft.AspNetCore.Mvc;
using smart_home_web.Models;
using smart_home_web.Models.ControlSensor;
using smart_home_web.Models.SensorViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace smart_home_web.Controllers
{
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

        [HttpGet]
        public IActionResult Index()
        {
            var data = _sensorControlManager.GetSensorControls();
             var model = _mapper.Map<List<SensorControlDto>,List<SensorControlViewModel>>(data);
            return View(model);       
        }

        [HttpPost]
        public IActionResult Change(int id, bool isActive)
        {
            var result = _sensorControlManager.UpdateById(id,isActive);
            if (!result.Succeeded) return BadRequest();
              
            return Ok();
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            var sensorControl = _sensorControlManager.GetById(id);
             var controlSensors = _sensorManager.GetControlSensors();
            var sensors = _sensorManager.GetSensorsToControl();

             if(sensorControl == null) return View("Error");

            var modelSensorControl = _mapper.Map<SensorControlDto,SensorControlViewModel>(sensorControl);
             var modelControlSensors = _mapper.Map<List<SensorDto>,List<SensorViewModel>>(controlSensors);
            var modelSensors = _mapper.Map<List<SensorDto>,List<SensorViewModel>>(sensors);
           

            return View(new IndexSensorControlViewModel {
                SensorControl = modelSensorControl,
                ControlSensorsView = modelControlSensors,
                SensorsView = modelSensors
            });;
            
        }

       
    }
}
