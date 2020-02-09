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
        SensorDto GetSensorByToken(Guid token);

		Task<List<SensorDto>> GetSensorsByReportElementType(ReportElementType type, int dashboardId);
        Task<SensorDto> GetSensorByIdAsync(int sensorId);

		Task<IEnumerable<SensorDto>> GetAllSensorsAsync();
        Task<IEnumerable<SensorDto>> GetAllSensorsByUserIdAsync(string userId);
        Task<SensorDto> GetLastSensor();

        List<SensorDto> GetSensorsToControl();
		List<SensorDto> GetControlSensors();

        Task<SensorDto> Create(SensorDto sensorDto);

		Task<SensorDto> Update(SensorDto sensorDto);
        Task<OperationDetails> Delete(int sensorId);
        OperationDetails AddUnclaimedSensor(Guid token, string value);
        Task SetActive(int id);
        
    }
}
