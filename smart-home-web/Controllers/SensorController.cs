using AutoMapper;
using Domain.Core.Model;
using Infrastructure.Business.DTOs.Sensor;
using Infrastructure.Business.Managers;
using Microsoft.AspNetCore.Mvc;
using smart_home_web.Models.SensorViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace smart_home_web.Controllers
{
    [Route("[controller]/[action]")]
    public class SensorController : Controller
    {
        private readonly IMapper _mapper;
        private readonly ISensorManager _sensorManager;

        public SensorController(IMapper mapper, ISensorManager sensorManager)
        {
            _sensorManager = sensorManager;
            _mapper = mapper;
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult AddSensor(CreateSensorViewModel sensor)
        {
            SensorDto sensorDto = _mapper.Map<CreateSensorViewModel, SensorDto>(sensor);
            _sensorManager.Insert(sensorDto);
            return RedirectToAction("Index", "Sensor");
        }

        [HttpGet]
        public IActionResult Index()
        {
            var sensors = _sensorManager.GetAllSensors();
            var models = _mapper.Map<IEnumerable<SensorDto>, IEnumerable<SensorViewModel>>(sensors);

            return View(new ListSensorViewModel
            {
                Sensors = models
            });
        }


    }
}
