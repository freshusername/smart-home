using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Infrastructure.Business.DTOs.Notification;
using Infrastructure.Business.Interfaces;
using Microsoft.AspNetCore.Mvc;
using smart_home_web.Models.Notification;

namespace smart_home_web.Controllers
{
	public class NotificationController : Controller
	{
		private readonly IToastManager _toastManager;
		private readonly IMapper _mapper;

		public NotificationController(IToastManager toastManager, IMapper mapper)
		{
			_toastManager = toastManager;
			_mapper = mapper;
		}

		public async Task<IActionResult> Index(int sensorId)
		{
			var notifications = await _toastManager.GetToastsBySensorId(sensorId);

			return View(_mapper.Map<IEnumerable<ToastDto>, IEnumerable<ToastViewModel>>(notifications));
		}

		public async Task<IActionResult> Read()
		{
			//var notifications = await _notificationManager.GetAllNotificationsAsync();
			//var models = _mapper.Map<IEnumerable<NotificationDto>, IEnumerable<NotificationViewModel>>(notifications);

			//return View(new AllNotificationsViewModel
			//{
			//	Notifications = models
			//});
			return View();
		}

		public async Task<IActionResult> ChangeStatus(int id, string page = "Index")
		{
			//await _notificationManager.ChangeStatusAsync(id);

			//return RedirectToAction(page);
			return View();
		}

		public ActionResult NotifForSensor(int sensorId)
		{
			ViewBag.SensorId = sensorId;
			return View();
		}

		public ActionResult Create(int sensorId)
		{
			CreateToastViewModel model = new CreateToastViewModel
			{
				SensorId = sensorId
			};
			return View(model);
		}

		public async Task<IActionResult> Detail(int id)
		{
			//var notitification = await _notificationManager.GetNotificationByIdAsync(id);

			//return View(_mapper.Map<NotificationDto, NotificationViewModel>(notitification));
			return View();
		}
	}
}