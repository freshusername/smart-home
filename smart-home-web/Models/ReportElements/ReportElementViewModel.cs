using Domain.Core.Model.Enums;
using System.Collections.Generic;

namespace smart_home_web.Models.ReportElements
{
	public class ReportElementViewModel
    {
        public int Id { get; set; }
        public int DashboardId { get; set; }
        public string DashboardName { get; set; }
        public int SensorId { get; set; }
        public string SensorName { get; set; }
        public ReportElementHours Hours { get; set; }
        public string Message { get; set; }
        public ReportElementType Type { get; set; }
        public bool IsCorrect { get; set; } = true;
        public bool IsActive { get; set; }

        public MeasurementType MeasurementType { get; set; }
        public string MeasurementName { get; set; }

        public string SensorType { get; set; }

        public List<dynamic> Values { get; set; }
        public List<long> Milliseconds { get; set; }

        public List<string> Dates { get; set; }
		public int X { get; set; }
		public int Y { get; set; }
		public int Weight { get; set; }
		public int Height { get; set; }
	
        public List<dynamic> MinValues { get; set; }
        public List<dynamic> MaxValues { get; set; }
    }
}

