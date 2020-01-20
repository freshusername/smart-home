using Domain.Core.Model;
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
        Task<IEnumerable<SensorDto>> GetAllSensorsAsync();
        OperationDetails AddUnclaimedSensor(Guid token, string value);
        Task<IEnumerable<SensorDto>> GetSensorsByReportElementType(ReportElementType type, int dashboardId);
    }
}
