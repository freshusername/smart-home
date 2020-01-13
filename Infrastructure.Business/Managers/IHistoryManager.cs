using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Domain.Core.Model.Enums;
using Infrastructure.Business.DTOs;
using Infrastructure.Business.DTOs.History;
using Infrastructure.Business.DTOs.ReportElements;
using Infrastructure.Business.DTOs.Sensor;
using Infrastructure.Business.Infrastructure;

namespace Infrastructure.Business.Managers
{
	public interface IHistoryManager
	{
		Task<HistoryDto> GetHistoryByIdAsync(int id);

		Task<IEnumerable<HistoryDto>> GetAllHistoriesAsync();
		Task<IEnumerable<HistoryDto>> GetHistoriesAsync(int count, int page, SortState sortState, bool IsActivated = true, int sensorId = 0);

		SensorDto GetSensorByToken(Guid token);

        OperationDetails AddHistory(string value, int sensorId);
		Task<IEnumerable<HistoryDto>> GetHistoriesBySensorIdAsync(int sensorId);

        Task<double?> GetMinValueAfterDate(int sensorId, DateTimeOffset dateTime);

        Task<double?> GetMaxValueAfterDate(int sensorId, DateTimeOffset dateTime);

		Task<GraphDTO> GetGraphBySensorId(int SensorId, int days);
		Task<int> GetAmountAsync(bool isActivated);
        Task<IEnumerable<HistoryDto>> GetInvalidSensors(SortState sortState);

    }
}
