using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Business.Hubs
{
    public interface IMessageHub
    {
        Task SendMessage(string name, string user, string message);
    }
}
