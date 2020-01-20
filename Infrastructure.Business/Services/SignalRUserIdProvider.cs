using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Business.Hubs
{
    public class SignalRUserIdProvider : IUserIdProvider
    {
        public string GetUserId(HubConnectionContext connection)
        {
            return connection.User?.Identity?.Name;
        }
    }
}
