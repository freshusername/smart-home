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
		Task<IEnumerable<History>> GetByPage(int count, int page, SortState sortState, int sensorId = 0);
		Task<int> GetAmountAsync();
	}
}
