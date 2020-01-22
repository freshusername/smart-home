using AutoMapper;
using Domain.Core.Model.Enums;
using Domain.Interfaces.Repositories;
using Infrastructure.Business.DTOs.Sensor;
using Infrastructure.Business.Hubs;
using Infrastructure.Business.Infrastructure;
using Infrastructure.Business.Interfaces;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Business.Managers
{
    public class MessageManager : BaseManager, IMessageManager
    {
        protected readonly IHubContext<MessageHub> messageHub;

        public MessageManager(IHubContext<MessageHub> hubcontext, IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork, mapper)
        {
            this.messageHub = hubcontext;
        }

        public async Task ShowMessage(Guid token, string value)
        {
            var sensor = unitOfWork.SensorRepo.GetByToken(token);
            string toasttype = "info";
            var notifications = await unitOfWork.NotificationRepo.GetBySensorId(sensor.Id);
            if(notifications.Any())
            {
                foreach(var notification in notifications)
                {
                    var incomingValue = ValueParser.Parse(value);
                    var ruleValue = ValueParser.Parse(notification.Value);
                    if (incomingValue.CompareTo(ruleValue) == (int)notification.Rule)
                    {
                        toasttype = notification.NotificationType.ToString().ToLower();
                    }
                    await messageHub.Clients.All.SendAsync("ShowToastMessage", toasttype, sensor.Name, value);
                }
            }
            else
                await messageHub.Clients.All.SendAsync("ShowToastMessage", "info", sensor.Name, value);
        }
    }
}
