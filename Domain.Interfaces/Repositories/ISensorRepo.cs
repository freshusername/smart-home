using Domain.Core.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Interfaces.Repositories
{
    public interface ISensorRepo : IGenericRepository<Sensor>
    {
        Sensor GetByToken(Guid token);
    }
}
