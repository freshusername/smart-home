using Infrastructure.Business.DTOs.Notification;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Business.Managers
{
	public interface INotificationManager
	{
		Task<NotificationDto> GetNotificationByIdAsync(int id);

		Task<IEnumerable<NotificationDto>> GetAllNotificationsAsync();
	}
}
