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

        // GET: SensorType/Details/5
        public ActionResult Details(int id)
        {
            return View();
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
            sensorType.Icon = await photoManager.GetPhotoFromFile(sensorTypeViewModel.Icon, 450, 450);
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
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: SensorType/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}