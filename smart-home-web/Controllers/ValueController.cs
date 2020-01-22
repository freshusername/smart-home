using Domain.Core.Model.Enums;
using Infrastructure.Business.Hubs;
using Infrastructure.Business.Infrastructure;
using Infrastructure.Business.Interfaces;
using Infrastructure.Business.Managers;
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
      
        public ValueController(IMessageManager messageManager, IHistoryManager historyTestManager, ISensorManager sensorManager , IActionService actionService)
        {
            _historyManager = historyTestManager;
            _messageManager = messageManager;
            _sensorManager = sensorManager;
            _actionService = actionService;
        }
        [HttpGet("getdata")]
        public async Task<IActionResult> AddHistory(Guid token, string value)
        {
            var sensor = _sensorManager.GetSensorByToken(token);
            if (sensor == null)
            {
              var result =  _sensorManager.AddUnclaimedSensor(token, value);
              if (result.Succeeded)
              {
                  result = _historyManager.AddHistory(value, Convert.ToInt32(result.Property));
                   return Ok(result.Message);
              } 
                                
               return BadRequest(result.Message);       
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
