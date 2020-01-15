using Domain.Core.Model;
using Domain.Core.Model.Enums;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Domain.Interfaces.Repositories
{
    public interface IHistoryRepo : IGenericRepository<History>
    {
		Task<IEnumerable<History>> GetHistoriesBySensorId(int SensorId);
        History GetLastHistoryBySensorId(int SensorId);
        //Task<double?> GetMinValueAfterDate(int SensorId, DateTimeOffset dateTime);
        //Task<double?> GetMaxValueAfterDate(int SensorId, DateTimeOffset dateTime);
        double? GetMinValueForPeriod(int sensorId, int? hours);
        double? GetMaxValueForPeriod(int sensorId, int? hours);
        Task<IEnumerable<History>> GetByPage(int count, int page, SortState sortState, bool isActivated = true, int sensorId = 0);
        Task<IEnumerable<History>> GetHistoriesBySensorIdAndDate(int SensorId, DateTime date);
        Task<int> GetAmountAsync(bool isActivated);
    }
}
