using System;

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
		public string UserId { get; set; }
		public string MeasurementName { get; set; }
		public string MeasurementType { get; set; }

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
