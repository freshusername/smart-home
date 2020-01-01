using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Infrastructure.Business.DTOs.SensorType;
using Infrastructure.Business.Managers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using smart_home_web.Models.SensorType;

namespace smart_home_web.Controllers
{
    public class SensorTypeController : Controller
    {
        private readonly ISensorTypeManager sensorTypeManager;
        private readonly IPhotoManager photoManager;
        private readonly IMapper mapper;

        public SensorTypeController(ISensorTypeManager _sensorTypeManager, IMapper _mapper, IPhotoManager _photoManager)
        {
            sensorTypeManager = _sensorTypeManager;
            mapper = _mapper;
            photoManager = _photoManager;
        }

        // GET: SensorType
        public async Task<ActionResult> Index()
        {
            var sensortypes = mapper.Map<IEnumerable<SensorTypeViewModel>>(await sensorTypeManager.GetAllSensorTypesAsync());
            return View(sensortypes);
        }

        // GET: SensorType/Create
        public ActionResult Create()
        {
            return View();
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
            var sensorType = mapper.Map<CreateSensorTypeViewModel, SensorTypeDto>(sensorTypeViewModel);
            if(sensorTypeViewModel.Icon != null)
                sensorType.Icon = await photoManager.GetPhotoFromFile(sensorTypeViewModel.Icon, 64, 64);
            var res = sensorTypeManager.Create(sensorType).Result;
            if (res.Succeeded)
                return RedirectToAction(nameof(Index));
            else
                ModelState.AddModelError(res.Property, res.Message);

            return View(sensorTypeViewModel);
        }

        // GET: SensorType/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: SensorType/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: SensorType/Delete/5
        public async Task<ActionResult> Delete(int id)
        {
            var sensorTypeDto = await sensorTypeManager.GetSensorTypeByIdAsync(id);
            SensorTypeViewModel sensorTypeViewModel = mapper.Map<SensorTypeDto, SensorTypeViewModel>(sensorTypeDto);
            return View(sensorTypeViewModel);
        }

        // POST: SensorType/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                var res = sensorTypeManager.Delete(id).Result;
                if (res.Succeeded)
                    return RedirectToAction(nameof(Index));
                else
                    ModelState.AddModelError(res.Property, res.Message);
                return View();
            }
            catch
            {
                return View();
            }
        }
    }
}