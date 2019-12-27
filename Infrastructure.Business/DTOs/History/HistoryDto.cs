using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Business.DTOs.History
{
	public class HistoryDto
	{
		public int Id { get; set; }
		public DateTimeOffset Date { get; set; }

		public string StringValue { get; set; }
		public int? IntValue { get; set; }
		public double? DoubleValue { get; set; }
		public bool? BoolValue { get; set; }

		public string SensorName { get; set; }
		public int SensorId { get; set; } // To pass particular sensor into GetHistoriesBySensorId()
		public string MeasurmentName { get; set; }
		public string MeasurmentType { get; set; }


		public string GetStringValue()
		{
			if (BoolValue != null) return BoolValue.ToString();
			if (IntValue.HasValue) return IntValue.ToString();
			if (DoubleValue.HasValue) return DoubleValue.ToString();
			if (!string.IsNullOrEmpty(StringValue)) return StringValue;
			else return "None";
		}
	}
}
