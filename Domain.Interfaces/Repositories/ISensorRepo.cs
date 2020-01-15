using Domain.Core.Model;
using System;

namespace Domain.Interfaces.Repositories
{
    public interface ISensorRepo : IGenericRepository<Sensor>
    {
        Sensor GetByToken(Guid token);
    }
}
