using Domain.Core.Model.Enums;
using Infrastructure.Business.Infrastructure;
using Infrastructure.Business.Managers;
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
        public ValueController(IHistoryManager historyTestManager ,ISensorManager sensorManager)
        {
            _historyTestManager = historyTestManager;
            _sensorManager = sensorManager;
        }
        [HttpGet("getdata")]
        public IActionResult AddHistory(Guid token, string value)
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
                return Ok(histroyResult.Message);

            return BadRequest(histroyResult.Message);        
        }
    }
}
