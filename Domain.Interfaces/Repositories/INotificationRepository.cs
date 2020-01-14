using Domain.Core.Model;
using System.Collections.Generic;

namespace Domain.Interfaces.Repositories
{
    public interface INotificationRepository : IGenericRepository<Message>
    {
        IEnumerable<Message> GetNotificationByHistoryId(int HistoryId);
    }
}
