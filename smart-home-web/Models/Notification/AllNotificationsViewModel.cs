using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace smart_home_web.Models.Notification
{
	public class AllNotificationsViewModel
	{
		public IEnumerable<NotificationViewModel> Notifications { get; set; }
	}
}
