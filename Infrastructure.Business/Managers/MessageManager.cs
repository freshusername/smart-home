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
using System.Text.RegularExpressions;
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
            var notifications = await unitOfWork.NotificationRepo.GetBySensorId(sensor.Id);
            if(notifications.Any())
            {
                foreach(var notification in notifications)
                {
                    var incomingValue = ValueParser.Parse(value);
                    var ruleValue = ValueParser.Parse(notification.Value);
                    if (incomingValue.CompareTo(ruleValue) == (int)notification.Rule)
                    {
                        string toasttype = notification.NotificationType.ToString().ToLower();
                        string message = notification.Message;
                        message = message.Replace("$Value$", value + sensor.SensorType.MeasurementName);
                        message = message.Replace("$SensorName$", sensor.Name);

                        await messageHub.Clients.All.SendAsync("ShowToastMessage", toasttype, message);
                    }
                }
            }
        }
    }
}
