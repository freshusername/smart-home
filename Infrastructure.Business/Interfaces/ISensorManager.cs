﻿using Domain.Core.Model;
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
        Task<OperationDetails> Create(SensorDto sensorDto);
        OperationDetails Update(SensorDto sensorDto);
        OperationDetails Delete(SensorDto sensorDto);
        Task<IEnumerable<SensorDto>> GetAllSensorsAsync();
        Task<SensorDto> GetSensorByIdAsync(int sensorId);
        OperationDetails AddUnclaimedSensor(Guid token, string value);
        Task<List<SensorDto>> GetSensorsByReportElementType(ReportElementType type, int dashboardId);
    }
}
