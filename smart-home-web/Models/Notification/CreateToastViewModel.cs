using Domain.Core.Model.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace smart_home_web.Models.Notification
{
    public class CreateToastViewModel
    {
		public int Id { get; set; }
		public RuleEnum Rule { get; set; }
		public ToastTypes NotificationType { get; set; }
		public string Value { get; set; }
		public string Message { get; set; } = "Sensor $SensorName$ returned value $Value$";

		public int SensorId { get; set; }
	}
}
