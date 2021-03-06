﻿using Infrastructure.Business.DTOs;
using Infrastructure.Business.Infrastructure;
using System.Collections.Generic;

namespace Infrastructure.Business.Interfaces
{
	public interface ISensorControlManager
    {
        List<SensorControlDto> GetSensorControls();

        SensorControlDto GetById(int id);

        OperationDetails Update(SensorControlDto controlDto);
        OperationDetails Add(SensorControlDto controlDto);
        OperationDetails UpdateById(int id, bool isActive);
        OperationDetails Delete(int id);
    }
}
