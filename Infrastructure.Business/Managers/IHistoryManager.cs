using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Infrastructure.Business.DTOs;
using Infrastructure.Business.DTOs.History;
using Infrastructure.Business.DTOs.Sensor;
using Infrastructure.Business.Infrastructure;

namespace Infrastructure.Business.Managers
{
	public interface IHistoryManager
	{
		Task<HistoryDto> GetHistoryByIdAsync(int id);

		Task<IEnumerable<HistoryDto>> GetAllHistoriesAsync();

        SensorDto GetSensorByToken(Guid token);

        OperationDetails AddHistory(string value, int sensorId);
        Task<IEnumerable<HistoryDto>> GetHistoriesBySensorIdAsync(int sensorId);

        double? GetMinValueAfterDate(int sensorId, DateTimeOffset dateTime);

        double? GetMaxValueAfterDate(int sensorId, DateTimeOffset dateTime);

        GraphDTO GetGraphBySensorId(int SensorId, int days);

    }
}
