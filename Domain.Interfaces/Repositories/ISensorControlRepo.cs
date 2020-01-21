using Domain.Core.Model;
using Domain.Core.Model.Enums;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces.Repositories
{
    public interface ISensorControlRepo : IGenericRepository<SensorControl>
    {
        Task<IEnumerable<SensorControl>> GetByToken(Guid token);
    }
}
