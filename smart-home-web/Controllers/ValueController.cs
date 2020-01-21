using Domain.Core.Model.Enums;
using Infrastructure.Business.Infrastructure;
using Infrastructure.Business.Managers;
using Infrastructure.Business.Services;
using Microsoft.AspNetCore.Mvc;
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
        private readonly ISensorManager _sensorManager;
        private readonly IActionService _actionService;
        public ValueController(IHistoryManager historyTestManager ,ISensorManager sensorManager , IActionService actionService)
        {
            _historyTestManager = historyTestManager;
            _sensorManager = sensorManager;
            _actionService = actionService;
        }
        [HttpGet("getdata")]
        public IActionResult AddHistory(Guid token, string value)
        {
            var sensor = _sensorManager.GetSensorByToken(token);
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
                return Ok(histroyResult.Message);

            return BadRequest(histroyResult.Message);        
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
