using Domain.Core.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces.Repositories
{
    public interface INotificationRepo : IGenericRepository<Notification>
    {
        Task<IEnumerable<Notification>> GetBySensorId(int sensorId);
    }
}
