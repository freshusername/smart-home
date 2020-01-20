﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Business.Interfaces
{
    public interface IMessageManager
    {
        Task ShowMessage(string name, string user, string message);
    }
}
