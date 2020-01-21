﻿using Infrastructure.Business.DTOs;
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
    }
}
