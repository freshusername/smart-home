using Infrastructure.Business.DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Business.Interfaces
{
    public interface ISensorControlManager
    {
        List<SensorControlDto> GetSensorControls();
    }
}
