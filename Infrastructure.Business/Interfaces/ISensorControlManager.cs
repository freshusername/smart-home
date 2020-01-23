using Infrastructure.Business.DTOs;
using Infrastructure.Business.DTOs.Sensor;
using Infrastructure.Business.Infrastructure;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Business.Interfaces
{
    public interface ISensorControlManager
    {
        List<SensorControlDto> GetSensorControls();
        SensorControlDto GetById(int id);
        OperationDetails Update(SensorControlDto controlDto);
        OperationDetails Add(SensorControlDto controlDto);
        OperationDetails UpdateById(int id, bool isActive);
    }
}
