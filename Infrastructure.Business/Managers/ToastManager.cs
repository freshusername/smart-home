using AutoMapper;
using Domain.Core.Model;
using Domain.Interfaces.Repositories;
using Infrastructure.Business.DTOs.Notification;
using Infrastructure.Business.Hubs;
using Infrastructure.Business.Infrastructure;
using Infrastructure.Business.Interfaces;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Infrastructure.Business.Interfaces
{
	public class ToastManager : BaseManager, IToastManager
    {
        protected readonly IHubContext<MessageHub> messageHub;

        public ToastManager(IHubContext<MessageHub> hubcontext, IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork, mapper)
        {
            this.messageHub = hubcontext;
        }

        public async Task<IEnumerable<ToastDto>> GetToastsBySensorId(int sensorId)
        {
            var notifications = await unitOfWork.NotificationRepo.GetBySensorId(sensorId);

            return mapper.Map<IEnumerable<Notification>, IEnumerable<ToastDto>>(notifications);
        }

        public async Task<ToastDto> GetById(int id)
        {
            var notifications = await unitOfWork.NotificationRepo.GetById(id);

            return mapper.Map<Notification, ToastDto>(notifications);
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

                        await messageHub.Clients.User(sensor.User.UserName)
                            .SendAsync("ShowToastMessage", toasttype, message);
                    }
                }
            }
        }

        public async Task<OperationDetails> Create(ToastDto toastDto)
        {
            var toast = mapper.Map<ToastDto, Notification>(toastDto);
            await unitOfWork.NotificationRepo.Insert(toast);
            var res = unitOfWork.Save();

            if (res > 0)
            {
                return new OperationDetails(true, "Saved successfully", "");
            }
            return new OperationDetails(true, "Something has gone wrong", "");
        }

        public async Task<OperationDetails> Update(ToastDto toastDto)
        {
            Notification toast = mapper.Map<ToastDto, Notification>(toastDto);
            try
            {
                await unitOfWork.NotificationRepo.Update(toast);
                unitOfWork.Save();
            }
            catch (Exception)
            {
                return new OperationDetails(false);
            }
            return new OperationDetails(true);
        }

        public async Task<OperationDetails> Delete(int id)
        {
            try
            {
                await unitOfWork.NotificationRepo.DeleteById(id);
                unitOfWork.Save();
            }
            catch
            {
                return new OperationDetails(false);
            }
            return new OperationDetails(true);
        }
    }
}
