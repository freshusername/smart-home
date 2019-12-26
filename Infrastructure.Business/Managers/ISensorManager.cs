using Infrastructure.Business.DTOs.Sensor;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Business.Managers
{
    public interface ISensorManager
    {
        Task<SensorDto> Insert(SensorDto sensorDto);
    }
}
