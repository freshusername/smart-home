using Domain.Core.Model;
using Domain.Core.Model.Enums;
using Infrastructure.Business.DTOs.Notification;
using Infrastructure.Business.Hubs;
using Infrastructure.Business.Infrastructure;
using Infrastructure.Business.Interfaces;
using Infrastructure.Business.Managers;
using Microsoft.AspNetCore.Identity;
using Infrastructure.Business.Services;
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
		private readonly IActionService _actionService;
		private readonly INotificationManager _notificationManager;
		private readonly UserManager<AppUser> _userManager;
		public ValueController(
			IMessageManager messageManager, 
			IHistoryManager historyTestManager, 
			ISensorManager sensorManager, 
			IActionService actionService, 
			INotificationManager notificationManager, 
			UserManager<AppUser> userManager)
		{
			_historyManager = historyTestManager;
            _actionService = actionService;
			_messageManager = messageManager;
			_sensorManager = sensorManager;
			_notificationManager = notificationManager;
			_userManager = userManager;
		}
		[HttpGet("getdata")]
		public async Task<IActionResult> AddHistory(Guid token, string value)
		{
			var sensor = _sensorManager.GetSensorByToken(token);
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

            var result = _actionService.CheckStatus(token).Result;
             if (result.Succeeded) return 1;


            return 0;
        }
    }
}
