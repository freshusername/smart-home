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
		public string SensorId { get; set; }
		public string MeasurementName { get; set; }
		public string MeasurementType { get; set; }
	}
}