using Domain.Core.Model.Enums;
using Infrastructure.Business.Hubs;
using Infrastructure.Business.Infrastructure;
using Infrastructure.Business.Interfaces;
using Infrastructure.Business.Managers;
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
        public ValueController(IMessageManager messageManager, IHistoryManager historyTestManager, ISensorManager sensorManager)
        {
            _historyManager = historyTestManager;
            _messageManager = messageManager;
            _sensorManager = sensorManager;
        }
        [HttpGet("getdata")]
        public async Task<IActionResult> AddHistory(Guid token, string value)
        {
            var sensor = _historyManager.GetSensorByToken(token);
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
            return (DateTime.Now.Second / 10) % 2;
        }
    }
}
