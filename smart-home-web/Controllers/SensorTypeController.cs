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
        private readonly IMapper mapper;

        public SensorTypeController(ISensorTypeManager _sensorTypeManager, IMapper _mapper)
        {
            sensorTypeManager = _sensorTypeManager;
            mapper = _mapper;
        }

        // GET: SensorType
        public ActionResult Index()
        {
            return View();
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
        public ActionResult Create(SensorTypeViewModel sensorTypeViewModel)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("Error", "Model State is not valid");
                return View(sensorTypeViewModel);
            }
            var sensorType = mapper.Map<SensorTypeViewModel, SensorTypeDto>(sensorTypeViewModel);
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