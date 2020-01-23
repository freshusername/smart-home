using Domain.Core.Model;
using Domain.Core.Model.Enums;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces.Repositories
{
    public interface ISensorRepo : IGenericRepository<Sensor>
    {
        Sensor GetByToken(Guid token);
        Task<Sensor> GetSensorById(int id);
        Task<IEnumerable<Sensor>> GetSensorsByMeasurementTypeAndUserId(MeasurementType type, string UserId);
       
    }
}
