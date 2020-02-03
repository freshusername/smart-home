using AutoMapper;
using Domain.Core.Model;
using Domain.Core.Model.Enums;
using Infrastructure.Business.DTOs.Sensor;
using Infrastructure.Business.DTOs.SensorType;
using Infrastructure.Business.Managers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using smart_home_web.Models.SensorType;
using smart_home_web.Models.SensorViewModel;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

namespace smart_home_web.Controllers
{
    [Route("[controller]/[action]")]
    public class SensorController : Controller
    {
        private readonly IMapper _mapper;
        private readonly ISensorManager _sensorManager;
        private readonly IIconManager _iconManager;
        private readonly ISensorTypeManager _sensorTypeManager;
        private readonly UserManager<AppUser> _userManager;
        private IHostingEnvironment _env;

        public SensorController(IMapper mapper,
            ISensorManager sensorManager,
            IIconManager iconManager,
            ISensorTypeManager sensorTypeManager,
            UserManager<AppUser> userManager,
            IHostingEnvironment env)
        {
            _sensorManager = sensorManager;
            _sensorTypeManager = sensorTypeManager;
            _iconManager = iconManager;
            _mapper = mapper;
            _userManager = userManager;
            _env = env;
        }

        [HttpGet]
        public async Task<ActionResult> Index()
        {
            if (User.Identity.IsAuthenticated)
            {
                string userId = _userManager.GetUserId(HttpContext.User);
                IEnumerable<SensorViewModel> sensors = _mapper.Map<IEnumerable<SensorViewModel>>(await _sensorManager.GetAllSensorsByUserIdAsync(userId));
                return View(sensors.Reverse());
            }

            return RedirectToAction("Login", "Account");
        }

        [HttpGet]
        [Authorize]
        public IActionResult Create()
        {
            IEnumerable<SensorTypeDto> sensorTypeDtos = _sensorTypeManager.GetAllSensorTypesAsync().Result;
            var sensorTypes = _mapper.Map<IEnumerable<SensorTypeDto>, IEnumerable<SensorTypeViewModel>>(sensorTypeDtos);
            ViewBag.sensorTypes = sensorTypes;
            ViewBag.loggedUserId = _userManager.GetUserId(HttpContext.User);
            return ViewComponent("SensorCreate");
        }

        [HttpPost]
        [Authorize]
        public async Task<ActionResult> Create(CreateSensorViewModel sensor)
        {
            SensorDto sensorDto = _mapper.Map<CreateSensorViewModel, SensorDto>(sensor);
            if (sensor.IconFile != null)
            {
                sensorDto.IconId = await _iconManager.CreateAndGetIconId(sensor.IconFile);
            }

            var res = _sensorManager.Create(sensorDto).Result;
            if (res.Succeeded)
            {
                SensorDto sensorfromDB = await _sensorManager.GetLastSensor();
                return ViewComponent("SensorElement", _mapper.Map<SensorDto, SensorViewModel>(sensorfromDB));
            }
            else
            {
                ModelState.AddModelError(res.Property, res.Message);
                return View(sensor);
            }
        }

        [Authorize]
        public async Task<ActionResult> Edit(int id)
        {
            var sensorDto = await _sensorManager.GetSensorByIdAsync(id);
            EditSensorViewModel sensorViewModel = _mapper.Map<SensorDto, EditSensorViewModel>(sensorDto);
            return ViewComponent("SensorEdit", sensorViewModel);
        }

        /*[Authorize]
        public async Task<ActionResult> Delete(int sensorId)
        {
            var sensorDto = await _sensorManager.GetSensorByIdAsync(sensorId);
            SensorViewModel sensorViewModel = _mapper.Map<SensorDto, SensorViewModel>(sensorDto);
            return View("Delete", sensorViewModel);
        }*/

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(EditSensorViewModel sensorViewModel)
        {
            SensorDto sensorDto = _mapper.Map<EditSensorViewModel, SensorDto>(sensorViewModel);
            if (sensorViewModel.IconFile != null)
            {
                sensorDto.IconId = await _iconManager.CreateAndGetIconId(sensorViewModel.IconFile);
            }
            try
            {
                _sensorManager.Update(sensorDto);
                SensorDto sensorfromDB = await _sensorManager.GetSensorByIdAsync(sensorDto.Id);
                return ViewComponent("SensorElement", _mapper.Map<SensorDto, SensorViewModel>(sensorfromDB));
            }
            catch
            {
                return View();
            }
        }

        [Authorize]
        public async Task<ActionResult> Delete(int Id)
        {
            try
            {
                var res = await _sensorManager.Delete(Id);
                if (!res.Succeeded)
                {
                    ModelState.AddModelError(res.Property, res.Message);
                    return View();
                }

                return Ok();
            }
            catch
            {
                return View();
            }
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetSensorsByReportElementType(ReportElementType type, int dashboardId)
        {
            var res = await _sensorManager.GetSensorsByReportElementType(type, dashboardId);
            if (res.Count == 0)
                return BadRequest();
            return Ok(res);
        }

        [HttpPost]
        public async Task<IActionResult> SetActive(int id)
        {
            await _sensorManager.SetActive(id);
            return Ok();
        }
    }
}
