using AutoMapper;
using Domain.Core.Model;
using Infrastructure.Business.DTOs.Icon;
using Infrastructure.Business.DTOs.Sensor;
using Infrastructure.Business.Managers;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using smart_home_web.Models.IconViewModel;
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

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddSensor(CreateSensorViewModel sensor)
        {
            if (sensor.IconFile != null)
            {
                string path = @"\images\SensorIcons";
                var uploadPath = _env.WebRootPath + path;

                if (!Directory.Exists(uploadPath))
                {
                    Directory.CreateDirectory(uploadPath);
                }

                using (var fileStream = new FileStream(Path.Combine(uploadPath, sensor.IconFile.FileName), FileMode.Create))
                {
                    await sensor.IconFile.CopyToAsync(fileStream);
                }

                var iconDto = new IconDto()
                {
                    Name = sensor.IconFile.FileName,
                    Path = uploadPath + sensor.IconFile.FileName
                };

                SensorDto sensorDto = _mapper.Map<CreateSensorViewModel, SensorDto>(sensor);
                sensorDto.IconId = _iconManager.InsertGetIconId(iconDto);

                _sensorManager.Insert(sensorDto);

            }




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
