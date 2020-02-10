using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Infrastructure.Business.DTOs.Notification;
using Infrastructure.Business.Interfaces;
using Infrastructure.Business.Managers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using smart_home_web.Models.Notification;

namespace smart_home_web.Controllers
{
    public class ToastController : Controller
    {
        private readonly IToastManager _toastManager;
        private readonly IMapper _mapper;

        public ToastController(IToastManager toastManager, IMapper mapper)
        {
            _toastManager = toastManager;
            _mapper = mapper;
        }

        public async Task<ActionResult> Index(int sensorId)
        {
            var toasts = await _toastManager.GetToastsBySensorId(sensorId);
            ViewBag.SensorId = sensorId;

            return View(_mapper.Map<IEnumerable<ToastDto>, IEnumerable<ToastViewModel>>(toasts));
        }

        public ActionResult Create(int sensorId)
        {
            CreateToastViewModel toastViewModel = new CreateToastViewModel() { SensorId = sensorId };
            return View(toastViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(CreateToastViewModel toastViewModel)
        {
            try
            {
                var toastDto = _mapper.Map<CreateToastViewModel, ToastDto>(toastViewModel);
                await _toastManager.Create(toastDto);

                return RedirectToAction("Index", "Toast", new { sensorId = toastViewModel.SensorId });
            }
            catch(Exception ex)
            {
                return View();
            }
        }

        public ActionResult Edit(int id)
        {
            return View();
        }

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

        public ActionResult Delete(int id)
        {
            return View();
        }

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