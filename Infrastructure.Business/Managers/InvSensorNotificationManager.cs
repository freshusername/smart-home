using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Domain.Core.Model;
using Domain.Interfaces;
using Domain.Interfaces.Repositories;
using Infrastructure.Business.DTOs.Notification;
using Infrastructure.Business.Hubs;
using Infrastructure.Business.Infrastructure;
using Infrastructure.Data;
using Microsoft.AspNetCore.SignalR;

namespace Infrastructure.Business.Interfaces
{
    public class InvSensorNotificationManager : BaseManager, IInvSensorNotificationManager
	{
		protected readonly IHubContext<MessageHub> messageHub;

		public InvSensorNotificationManager(IHubContext<MessageHub> hubcontext, IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork, mapper)
		{
			this.messageHub = hubcontext;
		}

		public async Task<NotificationDto> GetNotificationByIdAsync(int id)
		{
			//if (!this.unitOfWork.UserManager.GetUserId()
			//	return this.Redirect("/");

			var notification = await unitOfWork.MessageRepository.GetById(id);
			var result = mapper.Map<Message, NotificationDto>(notification);

			return result;
		}

		public async Task<IEnumerable<NotificationDto>> GetAllNotificationsAsync()
		{
			var notifications = await unitOfWork.MessageRepository.GetAll();
			var result = mapper.Map<IEnumerable<Message>, IEnumerable<NotificationDto>>(notifications);

			return result;
		}

		public async Task ChangeStatusAsync(int id)
		{
			var notification = await unitOfWork.MessageRepository.GetById(id);
			notification.IsRead = notification.IsRead ? false : true;
			unitOfWork.Save();
		}

		

		public async Task NotifyAboutInvalidSensor(int messageId)
		{
			var message = await unitOfWork.MessageRepository.GetById(messageId);
			await messageHub.Clients.User(message.AppUser.UserName)
							.SendAsync("NotifyAboutInvalidSensor", message);
		}

		public async Task<OperationDetails> CreateNotification(int historyId)
		{
			History history = await unitOfWork.HistoryRepo.GetByIdWithSensor(historyId);
			Message message = new Message();
			message.HistoryId = historyId;
			message.AppUserId = history.Sensor.AppUserId;
			try
			{
				await unitOfWork.MessageRepository.Insert(message);
				unitOfWork.Save();
			}
			catch(Exception ex)
			{
				return new OperationDetails(false, "Notification creation wasn't successfull", "", new Dictionary<string, object>() { { "id", message.Id } });
			}
			return new OperationDetails(true);
		}
	}
}
