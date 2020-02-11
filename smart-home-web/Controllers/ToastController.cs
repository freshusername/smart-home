using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Infrastructure.Business.DTOs.Notification;
using Infrastructure.Business.Interfaces;
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
            catch
            {
                return View();
            }
        }

        public async Task<ActionResult> Edit(int id)
        {
            var toastDto = await _toastManager.GetById(id);
            CreateToastViewModel toastViewModel = _mapper.Map<ToastDto, CreateToastViewModel>(toastDto);
            return View("Edit", toastViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(CreateToastViewModel toastViewModel)
        {
            ToastDto sensorTypeDto = _mapper.Map<CreateToastViewModel, ToastDto>(toastViewModel);
            var res = await _toastManager.Update(sensorTypeDto);

            if (res.Succeeded)
            {
                return RedirectToAction("Index", "Toast", new { sensorId = toastViewModel.SensorId });
            }
            else
            {
                return View();
            }
        }

        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                var res = await _toastManager.Delete(id);
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

    }
}