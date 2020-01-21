﻿using AutoMapper;
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

        public IActionResult Index(List<SensorControlViewModel> model)
        {
            var data = _sensorControlManager.GetSensorControls();
             model = _mapper.Map<List<SensorControlDto>,List<SensorControlViewModel>>(data);

            return View(model);
        }
    }
}
