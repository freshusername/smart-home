using Infrastructure.Business.DTOs.Notification;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Business.Interfaces
{
	public interface INotificationManager
	{
		Task<NotificationDto> GetNotificationByIdAsync(int id);
		Task ChangeStatusAsync(int id);

		Task<IEnumerable<NotificationDto>> GetAllNotificationsAsync();
	}
}
