using Domain.Core.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Interfaces
{
    public interface ISensorRepo : IGenericRepository<Sensor>
    {
        Sensor GetByToken(Guid token);
        Sensor GetSensorById(int id);
    }
}
