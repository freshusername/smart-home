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
        private readonly ISensorTypeManager _sensorTypeManager;
        private readonly IIconManager _iconManager;
        private readonly IPhotoManager photoManager;
        private readonly IMapper _mapper;

        public SensorTypeController(ISensorTypeManager sensorTypeManager, IMapper mapper, IIconManager iconManager)
        {
            _sensorTypeManager = sensorTypeManager;
            _mapper = mapper;
            //_photoManager = photoManager;
            _iconManager = iconManager;
        }

        // GET: SensorType
        public async Task<ActionResult> Index()
        {
            var sensortypes = _mapper.Map<IEnumerable<SensorTypeViewModel>>(await _sensorTypeManager.GetAllSensorTypesAsync());
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
            //if (!ModelState.IsValid)
            //{
            //    return View(sensorTypeViewModel);
            //}
            //var sensorType = mapper.Map<CreateSensorTypeViewModel, SensorTypeDto>(sensorTypeViewModel);
            //if(sensorTypeViewModel.Icon != null)
            //    sensorType.Icon = await photoManager.GetPhotoFromFile(sensorTypeViewModel.Icon, 64, 64);
            //var res = sensorTypeManager.Create(sensorType).Result;
            //if (res.Succeeded)
            //    return RedirectToAction(nameof(Index));
            //else
            //    ModelState.AddModelError(res.Property, res.Message);

            if (sensorTypeViewModel.IconFile != null)
            {

                SensorTypeDto sensorTypeDto = _mapper.Map<CreateSensorTypeViewModel, SensorTypeDto>(sensorTypeViewModel);
                sensorTypeDto.IconId = await _iconManager.CreateAndGetIconId(sensorTypeViewModel.IconFile);

                await _sensorTypeManager.Create(sensorTypeDto);
            }

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