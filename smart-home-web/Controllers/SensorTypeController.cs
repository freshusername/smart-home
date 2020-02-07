using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Infrastructure.Business.DTOs.SensorType;
using Infrastructure.Business.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using smart_home_web.Models.SensorType;

namespace smart_home_web.Controllers
{
    [Authorize]
    public class SensorTypeController : Controller
    {
        private readonly ISensorTypeManager _sensorTypeManager;
        private readonly IIconManager _iconManager;
        private readonly IMapper _mapper;

        public SensorTypeController(ISensorTypeManager sensorTypeManager, IMapper mapper, IIconManager iconManager)
        {
            _sensorTypeManager = sensorTypeManager;
            _mapper = mapper;
            _iconManager = iconManager;
        }

        // GET: SensorType
        public async Task<ActionResult> Index()
        {
            var sensortypes = _mapper.Map<IEnumerable<SensorTypeViewModel>>(await _sensorTypeManager.GetAllSensorTypesAsync());
            return View(sensortypes.Reverse());
        }

        // GET: SensorType/Create
        public ActionResult Create()
        {
            return ViewComponent("SensorTypeCreate");
        }

        // POST: SensorType/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(CreateSensorTypeViewModel sensorTypeViewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(sensorTypeViewModel);
            }
            SensorTypeDto sensorTypeDto = _mapper.Map<CreateSensorTypeViewModel, SensorTypeDto>(sensorTypeViewModel);

            if (sensorTypeViewModel.IconFile != null)
            {
                sensorTypeDto.IconId = await _iconManager.CreateAndGetIconId(sensorTypeViewModel.IconFile);
            }

            var res = _sensorTypeManager.Create(sensorTypeDto).Result;

            if (res.Succeeded)
            {
                SensorTypeDto sensorTypefromDB = await _sensorTypeManager.GetLastSensorType();
                return ViewComponent("SensorTypeElement", _mapper.Map<SensorTypeDto, SensorTypeViewModel>(sensorTypefromDB));
            }
            else
            {
                ModelState.AddModelError(res.Property, res.Message);
                return View(sensorTypeViewModel);
            }
 
        }

        // GET: SensorType/Edit/5
        public async Task<ActionResult> Edit(int id)
        {
            var sensorTypeDto = await _sensorTypeManager.GetSensorTypeByIdAsync(id);
            EditSensorTypeViewModel sensorTypeViewModel = _mapper.Map<SensorTypeDto, EditSensorTypeViewModel>(sensorTypeDto);
            return ViewComponent("SensorTypeEdit", sensorTypeViewModel);
        }

        // POST: SensorType/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(EditSensorTypeViewModel sensorTypeViewModel)
        {
            SensorTypeDto sensorTypeDto = _mapper.Map<EditSensorTypeViewModel, SensorTypeDto>(sensorTypeViewModel);
            if (sensorTypeViewModel.IconFile != null)
            {
                sensorTypeDto.IconId = await _iconManager.CreateAndGetIconId(sensorTypeViewModel.IconFile);
            }
            try
            {
                _sensorTypeManager.Update(sensorTypeDto);
                SensorTypeDto sensorTypefromDB = await _sensorTypeManager.GetSensorTypeByIdAsync(sensorTypeDto.Id);
                return ViewComponent("SensorTypeElement", _mapper.Map<SensorTypeDto, SensorTypeViewModel>(sensorTypefromDB));
            }
            catch
            {
                return View();
            }
        }

        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                var res = await _sensorTypeManager.Delete(id);
                if (!res.Succeeded) {
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
    }
}