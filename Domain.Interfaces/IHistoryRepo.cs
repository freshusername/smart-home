using Domain.Core.Model;
using Domain.Core.Model.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces
{
    public interface IHistoryRepo : IGenericRepository<History>
    {
		Task<IEnumerable<History>> GetHistoriesBySensorId(int SensorId);
        Task<double?> GetMinValueAfterDate(int SensorId, DateTimeOffset dateTime);
        Task<double?> GetMaxValueAfterDate(int SensorId, DateTimeOffset dateTime);
        Task<IEnumerable<History>> GetByPage(int count, int page, SortState sortState, bool isActivated = true, int sensorId = 0);
        Task<IEnumerable<History>> GetHistoriesBySensorIdAndDate(int SensorId, DateTime date);
        Task<int> GetAmountAsync(bool isActivated);
    }
		

}
