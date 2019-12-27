using Domain.Core.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Interfaces
{
    public interface IHistoryRepo : IGenericRepository<History>
    {
        IEnumerable<History> GetHistoriesBySensorId(int SensorId);
    }
}
