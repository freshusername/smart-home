using Infrastructure.Business.DTOs.SensorType;
using Infrastructure.Business.Infrastructure;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Business.Managers
{
    public interface ISensorTypeManager
    {
        Task<OperationDetails> Create(SensorTypeDto sensorTypeDto);
        Task<IEnumerable<SensorTypeDto>> GetAllSensorTypesAsync();
    }
}
