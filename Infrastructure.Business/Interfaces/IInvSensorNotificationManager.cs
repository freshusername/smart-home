using Infrastructure.Business.DTOs.Notification;
using Infrastructure.Business.Infrastructure;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Infrastructure.Business.Managers
{
	public interface IInvSensorNotificationManager
	{
		Task<NotificationDto> GetNotificationByIdAsync(int id);
		Task ChangeStatusAsync(int id);
		Task<IEnumerable<NotificationDto>> GetAllNotificationsAsync();

		Task<OperationDetails> CreateNotification(int historyId);

		Task NotifyAboutInvalidSensor(int messageId);
	}
}
