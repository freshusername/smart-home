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
			var notification = unitOfWork.NotificationRepository.GetById(id);
			var result = mapper.Map<Message, NotificationDto>(notification);

			return result;
		}

		public async Task<IEnumerable<NotificationDto>> GetAllNotificationsAsync()
		{
			var notifications = unitOfWork.NotificationRepository.GetAll().ToList();
			var result = mapper.Map<IEnumerable<Message>, IEnumerable<NotificationDto>>(notifications);

			return result;
		}
	}
}
