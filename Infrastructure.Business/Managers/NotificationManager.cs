using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Domain.Core.Model;
using Domain.Interfaces;
using Infrastructure.Business.DTOs.Notification;
using Infrastructure.Data;

namespace Infrastructure.Business.Managers
{
    public class NotificationManager : BaseManager, INotificationManager
	{
		public NotificationManager(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork, mapper)
		{

		}

		public async Task<NotificationDto> GetNotificationByIdAsync(int id)
		{
			//if (!this.unitOfWork.UserManager.GetUserId()
			//	return this.Redirect("/");

			var notification = await unitOfWork.NotificationRepository.GetById(id);
			var result = mapper.Map<Message, NotificationDto>(notification);

			return result;
		}

		public async Task<IEnumerable<NotificationDto>> GetAllNotificationsAsync()
		{
			var notifications = await unitOfWork.NotificationRepository.GetAll();
			var result = mapper.Map<IEnumerable<Message>, IEnumerable<NotificationDto>>(notifications);

			return result;
		}

		public async Task ChangeStatusAsync(int id)
		{
			var notification = await unitOfWork.NotificationRepository.GetById(id);
			notification.IsRead = notification.IsRead ? false : true;
			unitOfWork.Save();
		}
	}
}
