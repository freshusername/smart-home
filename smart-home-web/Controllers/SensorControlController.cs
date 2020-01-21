using AutoMapper;
using Infrastructure.Business.DTOs;
using Infrastructure.Business.Interfaces;
using Microsoft.AspNetCore.Mvc;
using smart_home_web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace smart_home_web.Controllers
{
    public class SensorControlController : Controller
    {
        private readonly ISensorControlManager _sensorControlManager;
        private readonly IMapper _mapper;

        public SensorControlController(ISensorControlManager sensorControlManager , IMapper mapper)
        {
            _sensorControlManager = sensorControlManager;
            _mapper = mapper;
        }

        public IActionResult Index()
        {
            var data = _sensorControlManager.GetSensorControls();
             var model = _mapper.Map<List<SensorControlDto>,List<SensorControlViewModel>>(data);
            return View(model);
        }

        [HttpGet]
        public IActionResult Change(int id)
        {
            var data = _sensorControlManager.GetById(id);
             var model = _mapper.Map<SensorControlDto,SensorControlViewModel>(data);
            return View(model);
        }
    }
}
