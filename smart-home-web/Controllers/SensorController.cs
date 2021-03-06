﻿using AutoMapper;
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
using Infrastructure.Business.DTOs.Icon;

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
        public async Task<ActionResult> Create(CreateSensorViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            SensorDto sensorDto = _mapper.Map<CreateSensorViewModel, SensorDto>(model);
            SensorTypeDto sensorType = await _sensorTypeManager.GetSensorTypeByIdAsync(sensorDto.SensorTypeId);
            if (model.IconFile != null)
            {
                sensorDto.IconId = await _iconManager.CreateAndGetIconId(model.IconFile);
            }
            else
            {

                sensorDto.IconId = sensorType.IconId;
            }

            var res = await _sensorManager.Create(sensorDto);

            if (res != null)
            {
                res.SensorTypeName = sensorType.Name;
                return ViewComponent("SensorElement", _mapper.Map<SensorDto, SensorViewModel>(res));
            }
            else
            {
                //ModelState.AddModelError(res.Property, res.Message);
                return View(model);
            }
        }

        [Authorize]
        public async Task<ActionResult> Edit(int id)
        {
            var sensorDto = await _sensorManager.GetSensorByIdAsync(id);
            if (sensorDto != null)
            {
                EditSensorViewModel sensorViewModel = _mapper.Map<SensorDto, EditSensorViewModel>(sensorDto);
                return ViewComponent("SensorEdit", sensorViewModel);
            }
            return ViewComponent("SensorEdit", null);

        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(EditSensorViewModel model)
        {
            SensorDto sensorDto = _mapper.Map<EditSensorViewModel, SensorDto>(model);
            IconDto iconDto = null;
            if (model.IconFile != null)
            {
                sensorDto.IconId = _iconManager.CreateAndGetIconId(model.IconFile).Result;
                iconDto = await _iconManager.GetById(sensorDto.IconId.GetValueOrDefault());
                sensorDto.IconPath = iconDto.Path;
            }

            var res = await _sensorManager.Update(sensorDto);

            if (res != null)
            {
                res = await _sensorManager.GetSensorByIdAsync(sensorDto.Id);
                iconDto = await _iconManager.GetById(sensorDto.IconId.GetValueOrDefault());
                sensorDto.IconPath = iconDto.Path;
                sensorDto.SensorTypeName = res.SensorTypeName;
                return ViewComponent("SensorElement", _mapper.Map<SensorDto, SensorViewModel>(sensorDto));
            }
            else
            {
                //ModelState.AddModelError(res.Property, res.Message);
                return View(model);
            }
        }

        [Authorize]
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                var res = await _sensorManager.Delete(id);
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
