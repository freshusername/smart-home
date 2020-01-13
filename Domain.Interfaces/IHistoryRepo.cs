using Domain.Core.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Interfaces
{
    public interface IHistoryRepo : IGenericRepository<History>
    {
        IEnumerable<History> GetHistoriesBySensorId(int SensorId);
        History GetLastHistoryBySensorId(int SensorId);
        double? GetMinValueAfterDate(int SensorId, DateTimeOffset dateTime);
        double? GetMaxValueAfterDate(int SensorId, DateTimeOffset dateTime);
    }
}
