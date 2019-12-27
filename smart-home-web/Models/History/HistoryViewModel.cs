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

		public string Value { get; set; }
		
		public string SensorName { get; set; }
		public string MeasurmentName { get; set; }
		public string MeasurmentType { get; set; }
	}
}