using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Business.Hubs
{
    public class GraphHub : Hub
    {
        public async Task UpdateGraph(int sensorId, string value, long date)
        {
            await Clients.All.SendAsync("UpdateGraph");
        }
    }
}