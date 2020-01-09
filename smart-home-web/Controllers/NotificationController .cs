using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Infrastructure.Business.DTOs.Notification;
using Infrastructure.Business.Managers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Migrations;
using smart_home_web.Models.Notification;

namespace smart_home_web.Controllers
{
	public class NotificationController : Controller
	{
		private readonly INotificationManager _notificationManager;
		private readonly IMapper _mapper;

		public NotificationController(INotificationManager notificationManager, IMapper mapper)
		{
			_notificationManager = notificationManager;
			_mapper = mapper;
		}

		public async Task<IActionResult> Index()
		{
			var notifications = await _notificationManager.GetAllNotificationsAsync();
			var models = _mapper.Map<IEnumerable<NotificationDto>, IEnumerable<NotificationViewModel>>(notifications);

			return View(new AllNotificationsViewModel
			{
				Notifications = models
			});
		}
		public async Task<IActionResult> Read()
		{
			var notifications = await _notificationManager.GetAllNotificationsAsync();
			var models = _mapper.Map<IEnumerable<NotificationDto>, IEnumerable<NotificationViewModel>>(notifications);

			return View(new AllNotificationsViewModel
			{
				Notifications = models
			});
		}

		public async Task<IActionResult> ChangeStatus(int id, string page = "Index")
		{
			await _notificationManager.ChangeStatusAsync(id);

			return RedirectToAction(page);
		}

		public async Task<IActionResult> All()
		{
			var notifications = await _notificationManager.GetAllNotificationsAsync();
			var models = _mapper.Map<IEnumerable<NotificationDto>, IEnumerable<NotificationViewModel>>(notifications);

			return View(new AllNotificationsViewModel
			{
				Notifications = models
			});
		}

		public async Task<IActionResult> Detail(int id)
		{
			var notitification = await _notificationManager.GetNotificationByIdAsync(id);

			return View(_mapper.Map<NotificationDto, NotificationViewModel>(notitification));
		}
	}
}