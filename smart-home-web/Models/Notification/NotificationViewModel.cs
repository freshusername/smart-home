using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace smart_home_web.Models.Notification
{
	public class NotificationViewModel
	{
		public int Id { get; set; }
		public string Comment { get; set; }
		public bool IsRead { get; set; }
		public string UserName { get; set; }
		public DateTimeOffset Date { get; set; }
	}
}