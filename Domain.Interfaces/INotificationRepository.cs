using Domain.Core.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Interfaces
{
    public interface INotificationRepository : IGenericRepository<Message>
    {
        IEnumerable<Message> GetNotificationByHistoryId(int HistoryId);
    }
}
