using Domain.Core.Model.Enums;

namespace smart_home_web.Models.ReportElements
{
	public class GaugeViewModel
    {
        public int Id { get; set; }
        public int SensorId { get; set; }
        public string SensorName { get; set; }
        public ReportElementHours Hours { get; set; }
        public string MeasurementName { get; set; }
        public bool IsValid { get; set; }
        public double Value { get; set; }
        public int Min { get; set; }
        public int Max { get; set; }
    }
}
