using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Infrastructure.Business.DTOs;
using Infrastructure.Business.DTOs.History;
using Infrastructure.Business.DTOs.Sensor;

namespace Infrastructure.Business.Managers
{
	public interface IHistoryManager
	{
		Task<HistoryDto> GetHistoryByIdAsync(int id);

		Task<IEnumerable<HistoryDto>> GetAllHistoriesAsync();
		Task<IEnumerable<HistoryDto>> GetHistoriesBySensorIdAsync(int sensorId);

        GraphDTO GetGraphBySensorId(int SensorId, int days);
    }
}
