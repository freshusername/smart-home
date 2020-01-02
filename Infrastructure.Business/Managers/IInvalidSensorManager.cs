using System;
using System.Collections.Generic;
using System.Text;
using Infrastructure.Business.DTOs.History;
using Domain.Core.Model.Enums;

namespace Infrastructure.Business.Managers
{
    public interface IInvalidSensorManager
    {
        IEnumerable<HistoryDto> getInvalidSensors(SortState sortState);
    }
}
