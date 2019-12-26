using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace smart_home_web.Models.History
{
	public class HistoryViewModel
	{
		public int Id { get; set; }
		public DateTimeOffset Date { get; set; }

		public string StringValue { get; set; }
		public int? IntValue { get; set; }
		public double? DoubleValue { get; set; }
		public bool? BoolValue { get; set; }

		public int? SensorId { get; set; }
	}
}