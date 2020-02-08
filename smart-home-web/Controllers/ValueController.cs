using Domain.Core.Model;
using Domain.Core.Model.Enums;
using Infrastructure.Business.DTOs.Notification;
using Infrastructure.Business.Hubs;
using Infrastructure.Business.Infrastructure;
using Infrastructure.Business.Interfaces;
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
        private readonly IToastManager _messageManager;
        private readonly ISensorManager _sensorManager;
        private readonly IActionService _actionService;
        private readonly INotificationManager _notificationManager;
        private readonly IEmailSender _emailSender;
        private readonly UserManager<AppUser> _userManager;
        public ValueController(
            IToastManager messageManager,
            IHistoryManager historyTestManager,
            ISensorManager sensorManager,
            IActionService actionService,
            INotificationManager notificationManager,
            UserManager<AppUser> userManager,
            IEmailSender emailSender)
        {
            _historyManager = historyTestManager;
            _actionService = actionService;
            _messageManager = messageManager;
            _sensorManager = sensorManager;
            _notificationManager = notificationManager;
            _userManager = userManager;
            _emailSender = emailSender;
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
                await _historyManager.UpdateGraph(token, value);
				await _messageManager.ShowMessage(token, value);
				return Ok(historyResult.Message);
			}

            return BadRequest(historyResult.Message);
        }

        [HttpGet("getaction")]
        public int GetAction(Guid token)
        {
            var result = _actionService.CheckStatus(token).Result;
            if (!result.Succeeded)
                return 0;

            var sensor = _sensorManager.GetSensorByToken(token);
             var userEmail = _userManager.FindByIdAsync(sensor.AppUserId).Result.Email;
            var date = DateTime.Now.ToLocalTime();
             _emailSender.SendEmailAsync(userEmail, "🏠Smart home", $"<span style=\"font-size: 20px\">Sensor : <b>{sensor.Name}</b>.<br/>Value : <b>true</b>❗.<br/>Date : {date}</span>");
            return 1;
        }

        [HttpGet("alexaresponse")]
        public  IActionResult GetResponse(Guid controlToken, Guid sensorToken , bool isActive)
        {

            var result = _actionService.Activate(controlToken , sensorToken, isActive);
             if (!result.Succeeded) return BadRequest();

            return Ok();

        }
    }
}
