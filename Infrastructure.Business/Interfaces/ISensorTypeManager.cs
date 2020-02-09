using Infrastructure.Business.DTOs.SensorType;
using Infrastructure.Business.Infrastructure;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Business.Interfaces
{
    public interface ISensorTypeManager
    {
        Task<SensorTypeDto> Create(SensorTypeDto sensorTypeDto);
        Task<OperationDetails> Delete(int id);

        Task<SensorTypeDto> Update(SensorTypeDto sensorTypeDto);

		Task<SensorTypeDto> GetSensorTypeByIdAsync(int id);
        Task<SensorTypeDto> GetLastSensorType();


        Task<IEnumerable<SensorTypeDto>> GetAllSensorTypesAsync();
    }
}
