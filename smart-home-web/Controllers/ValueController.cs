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
using Infrastructure.Business.Managers;

namespace smart_home_web.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ValueController : ControllerBase
    {
        private static DateTimeOffset _date;
        private readonly IHistoryManager _historyManager;
        private readonly IToastManager _toastManager;
        private readonly ISensorManager _sensorManager;
        private readonly IActionService _actionService;
        private readonly IInvSensorNotificationManager _notificationManager;
        private readonly IEmailSender _emailSender;
        private readonly UserManager<AppUser> _userManager;
        public ValueController(
            IToastManager messageManager,
            IHistoryManager historyTestManager,
            ISensorManager sensorManager,
            IActionService actionService,
            IInvSensorNotificationManager notificationManager,
            UserManager<AppUser> userManager,
            IEmailSender emailSender)
        {
            _historyManager = historyTestManager;
            _actionService = actionService;
            _toastManager = messageManager;
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
                    result = _historyManager.AddHistory(value, Convert.ToInt32(result.Data["id"]));
                    if(result.Succeeded)
                        result = await _notificationManager.CreateNotification(Convert.ToInt32(result.Data["id"]));
                    if(result.Succeeded)
                        await _notificationManager.NotifyAboutInvalidSensor(Convert.ToInt32(result.Data["id"]));
                    return Ok(result.Message);
                }
            }

            var historyResult = _historyManager.AddHistory(value, sensor.Id);

			if (historyResult.Succeeded)
			{
                await _historyManager.UpdateGraph(token, value);
				await _toastManager.ShowMessage(token, value);
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

            if (_date == default(DateTimeOffset))
            {
                _date = DateTimeOffset.Now;
                SendEmail(token);
            }
            if (_date < DateTimeOffset.Now.AddMinutes(-5))
                SendEmail(token);

            return 1;
        }

        [HttpGet("alexaresponse")]
        public  IActionResult GetResponse(Guid controlToken, Guid sensorToken , bool isActive)
        {

            var result = _actionService.Activate(controlToken , sensorToken, isActive);
             if (!result.Succeeded) return BadRequest();

            return Ok();

        }

        private void SendEmail(Guid token)
        {
            var sensor = _sensorManager.GetSensorByToken(token);
            var userEmail = _userManager.FindByIdAsync(sensor.AppUserId).Result.Email;
            var date = DateTime.Now.ToLocalTime();
            _emailSender.SendEmailAsync(userEmail, "🏠Smart-home", $"<span style=\"font-size: 20px\">Sensor : <b>{sensor.Name}</b>.<br/>Value : <b>true</b>❗.<br/>Date : {date}</span>");
        }
    }
}
