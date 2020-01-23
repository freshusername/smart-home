using Domain.Core.Model;
using Domain.Core.Model.Enums;
using Infrastructure.Business.DTOs.Notification;
using Infrastructure.Business.Hubs;
using Infrastructure.Business.Infrastructure;
using Infrastructure.Business.Interfaces;
using Infrastructure.Business.Managers;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace smart_home_web.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public class ValueController : ControllerBase
	{
		private readonly IHistoryManager _historyManager;
		private readonly IMessageManager _messageManager;
		private readonly ISensorManager _sensorManager;
		private readonly INotificationManager _notificationManager;
		private readonly UserManager<AppUser> _userManager;
		public ValueController(
			IMessageManager messageManager, 
			IHistoryManager historyTestManager, 
			ISensorManager sensorManager, 
			INotificationManager notificationManager, 
			UserManager<AppUser> userManager)
		{
			_historyManager = historyTestManager;
			_messageManager = messageManager;
			_sensorManager = sensorManager;
			_notificationManager = notificationManager;
			_userManager = userManager;
		}
		[HttpGet("getdata")]
		public async Task<IActionResult> AddHistory(Guid token, string value)
		{
			var sensor = _historyManager.GetSensorByToken(token);
			if (sensor == null)
			{
				var result = _sensorManager.AddUnclaimedSensor(token, value);
				if (result.Succeeded)
				{
					result = _historyManager.AddHistory(value, Convert.ToInt32(result.Property));
					return Ok(result.Message);
				}

				//TODO: Notification logic 

				//var unclaimedSensor = _historyManager.GetSensorByToken(token);
				//var history = _historyManager.GetLastHistoryBySensorId(unclaimedSensor.Id);
				//var notification = new NotificationDto
				//{
				//	Comment = "Unknown sensor sended a value",
				//	Date = history.Date,
				//	Id = history.Id,
				//	IsRead = false,
				//	UserName = 
				//}
				//await _notificationManager.Create(history);
				//return BadRequest(result.Message);
			}

			var historyResult = _historyManager.AddHistory(value, sensor.Id);

			if (historyResult.Succeeded)
			{
				await _messageManager.ShowMessage(token, value);
				return Ok(historyResult.Message);
			}

			return BadRequest(historyResult.Message);
		}

		[HttpGet("getaction")]
		public int GetAction(Guid token)
		{
			//return (DateTime.Now.Second / 10) % 2;
			return 0;
		}
	}
}
