using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Business.DTOs.Notification
{
	public class NotificationDto
	{
		public int Id { get; set; }
		public string Comment { get; set; }
		public bool IsRead { get; set; }
		public string UserName { get; set; }
		public DateTimeOffset Date { get; set; }
	}
}
