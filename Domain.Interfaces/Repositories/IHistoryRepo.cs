using Domain.Core.CalculateModel;
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
        Task<History> GetLastBySensorId(int sensorId);
        //Task<double?> GetMinValueAfterDate(int SensorId, DateTimeOffset dateTime);
        //Task<double?> GetMaxValueAfterDate(int SensorId, DateTimeOffset dateTime);
        double? GetMinValueForPeriod(int sensorId, int? hours);
        double? GetMaxValueForPeriod(int sensorId, int? hours);
        int? GetIntMinValueForPeriod(int sensorId, int? minutes);
        int? GetIntMaxValueForPeriod(int sensorId, int? minutes);
        Task<IEnumerable<History>> GetByPage(int count, int page, SortState sortState, bool isActivated = true, int sensorId = 0);
        Task<IEnumerable<History>> GetHistoriesBySensorIdAndDate(int SensorId, DateTimeOffset date);
        Task<IEnumerable<History>> GetHistoriesBySensorIdAndDatePeriod(int SensorId, DateTime dateFrom, DateTime dateTo);
        Task<IEnumerable<AvgSensorValuePerDay>> GetAvgSensorsValuesPerDays(int sensorId, DateTime dateFrom, DateTime dateTo);
        Task<int> GetAmountAsync(bool isActivated);
        History GetLastHistoryBySensorIdAndDate(int sensorId, DateTimeOffset date);
    }
}
