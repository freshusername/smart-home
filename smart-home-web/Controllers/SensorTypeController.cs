using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Infrastructure.Business.DTOs.Icon;
using Infrastructure.Business.DTOs.SensorType;
using Infrastructure.Business.Interfaces;
using Infrastructure.Business.Managers;
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

            if (res != null) 
            {
                return ViewComponent("SensorTypeElement", _mapper.Map<SensorTypeDto, SensorTypeViewModel>(res));
            }
            else
            {
                //ModelState.AddModelError(res.Property, res.Message);
                return View(sensorTypeViewModel);
            }
 
        }

        // GET: SensorType/Edit/5
        public async Task<ActionResult> Edit(int id)
        {
            var sensorTypeDto = await _sensorTypeManager.GetSensorTypeByIdAsync(id);
            if (sensorTypeDto != null)
            {
                EditSensorTypeViewModel sensorTypeViewModel = _mapper.Map<SensorTypeDto, EditSensorTypeViewModel>(sensorTypeDto);
                return ViewComponent("SensorTypeEdit", sensorTypeViewModel);
            }
            return ViewComponent("SensorTypeEdit", null);

        }

        // POST: SensorType/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(EditSensorTypeViewModel sensorTypeViewModel)
        {
            SensorTypeDto sensorTypeDto = _mapper.Map<EditSensorTypeViewModel, SensorTypeDto>(sensorTypeViewModel);
            IconDto iconDto = null;
            if (sensorTypeViewModel.IconFile != null)
            {
                sensorTypeDto.IconId = await _iconManager.CreateAndGetIconId(sensorTypeViewModel.IconFile);
                iconDto = await _iconManager.GetById(sensorTypeDto.IconId.GetValueOrDefault());
                sensorTypeDto.IconPath = iconDto.Path;
            }
            var res = await _sensorTypeManager.Update(sensorTypeDto);

            if (res != null)
            {
                //res = await _sensorTypeManager.GetSensorTypeByIdAsync(sensorTypeDto.Id);
                iconDto = await _iconManager.GetById(sensorTypeDto.IconId.GetValueOrDefault());
                sensorTypeDto.IconPath = iconDto.Path;
                return ViewComponent("SensorTypeElement", _mapper.Map<SensorTypeDto, SensorTypeViewModel>(sensorTypeDto));
            }
            else
            {
                //ModelState.AddModelError(res.Property, res.Message);
                return View(sensorTypeViewModel);
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