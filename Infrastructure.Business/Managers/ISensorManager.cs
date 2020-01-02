using Domain.Core.Model.Enums;
using Infrastructure.Business.DTOs.Sensor;
using Infrastructure.Business.Infrastructure;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Business.Managers
{
    public interface ISensorManager
    {
        void Insert(SensorDto sensorDto);
        IEnumerable<SensorDto> GetAllSensors();
        OperationDetails AddUnclaimedSensor(Guid token, MeasurmentType? mesurmentType);       
    }
}
