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
        private readonly IHistoryTestManager _historyTestManager;
        private readonly ISensorManager _sensorManager;
        public ValueController(IHistoryTestManager historyTestManager ,ISensorManager sensorManager)
        {
            _historyTestManager = historyTestManager;
            _sensorManager = sensorManager;
        }
        [HttpGet("getdata")]
        public IActionResult AddHistory(Guid token, string value)
        {

            var sensor = _historyTestManager.GetSensorByToken(token);
            MeasurmentType? measurment = null;
            int intValue;
            double doubleValue;    
            bool boolValue;

            if (sensor == null)
            {

                if (int.TryParse(value , out intValue))
                    measurment = MeasureType<int>.GetMeasureType(intValue);

                if (double.TryParse(value, out doubleValue))
                    measurment = MeasureType<double>.GetMeasureType(doubleValue);

                if (bool.TryParse(value, out boolValue))
                    measurment = MeasureType<bool>.GetMeasureType(boolValue);

                    measurment = MeasureType<string>.GetMeasureType(value);

                var result = _sensorManager.AddUnclaimedSensor(token, measurment);

                if (result.Succeeded)
                    return Ok(result.Property);

                return BadRequest(result.Message);
            }

            var histroyResult = _historyTestManager.AddHistory(value, sensor.Id);

            if (histroyResult.Succeeded)
                return Ok(histroyResult.Property);

            return BadRequest(histroyResult.Message);
           
        }
    }
}
