using System;
using System.Collections.Generic;
using System.Text;
using Infrastructure.Business.DTOs.History;
using Domain.Core.Model;

namespace Infrastructure.Business.Managers
{
    public interface IInvalidSensorManager
    {
        List<HistoryDto> getInvalidSensors();
    }
}
