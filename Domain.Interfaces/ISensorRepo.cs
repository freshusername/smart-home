using Domain.Core.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces
{
    public interface ISensorRepo : IGenericRepository<Sensor>
    {
        Sensor GetByToken(Guid token);
        Task<Sensor> GetSensorById(int id);
    }
}
