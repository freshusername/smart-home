using System;
using System.Collections.Generic;
using System.Text;
using Infrastructure.Business.DTOs.History;
using Domain.Core.Model.Enums;
using System.Threading.Tasks;

namespace Infrastructure.Business.Managers
{
    public interface IInvalidSensorManager
    {
        Task<IEnumerable<HistoryDto>> getInvalidSensors(SortState sortState);
    }
}
