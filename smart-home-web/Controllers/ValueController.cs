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
        private readonly IHistoryManager _historyTestManager;
        private readonly IMessageManager _messageManager;
        private readonly ISensorManager _sensorManager;
        public ValueController(IHubContext<MessageHub> messageHub, IMessageManager messageManager, IHistoryManager historyTestManager ,ISensorManager sensorManager)
        {
            _historyTestManager = historyTestManager;
            _messageManager = messageManager;
            _sensorManager = sensorManager;
        }
        [HttpGet("getdata")]
        public async Task<IActionResult> AddHistory(Guid token, string value)
        {
            var sensor = _historyTestManager.GetSensorByToken(token);
            if (sensor == null)
            {
              var result =  _sensorManager.AddUnclaimedSensor(token , value);
              if (result.Succeeded)
              {
                  result = _historyTestManager.AddHistory(value, Convert.ToInt32(result.Property));
                   return Ok(result.Message);
              } 
                                
               return BadRequest(result.Message);       
            }
             
            var histroyResult = _historyTestManager.AddHistory(value, sensor.Id);

            if (histroyResult.Succeeded)
            {
                await _messageManager.ShowMessage("ShowToastMessage", sensor.Name, value);
                return Ok(histroyResult.Message);
            }

            return BadRequest(histroyResult.Message);        
        }

        [HttpGet("getaction")]
        public int GetAction(Guid token)
        {
            return (DateTime.Now.Second / 10) % 2;
        }
    }
}
