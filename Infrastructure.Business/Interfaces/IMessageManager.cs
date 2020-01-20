using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Business.Interfaces
{
    public interface IMessageManager
    {
        void ShowMessage(string name, string user, string message);
    }
}
