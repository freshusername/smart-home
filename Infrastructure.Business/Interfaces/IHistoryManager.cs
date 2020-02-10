using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Domain.Core.Model.Enums;
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
		Task<IEnumerable<HistoryDto>> GetHistoriesAsync(int count, int page, SortState sortState, bool IsActivated, int sensorId = 0);
		Task<IEnumerable<HistoryDto>> GetHistoriesBySensorIdAsync(int sensorId);

		HistoryDto GetLastHistoryBySensorId(int sensorId);
        OperationDetails AddHistory(string value, int sensorId);

        Task<SensorDto> GetLastSensorByUserId(string userId);


        double? GetMinValueForPeriod(int sensorId, int? hours);
		double? GetMaxValueForPeriod(int sensorId, int? hours);

		Task<GraphDto> GetGraphBySensorId(int SensorId, int days);

		Task<int> GetAmountAsync(bool isActivated);
		Task<int> GetAmountOfUserHistoriesAsync(bool isActivated, string userId);
        Task UpdateGraph(Guid token, string value);
    }
}
