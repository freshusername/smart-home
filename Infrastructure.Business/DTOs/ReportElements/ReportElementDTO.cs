using Domain.Core.Model.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Business.DTOs.ReportElements
{
    public class ReportElementDTO
    {
        public int Id { get; set; }

        public int DashboardId { get; set; }
        public string DashboardName { get; set; }

        public int SensorId { get; set; }
        public string SensorName { get; set; }

        public int Hours { get; set; }
        public ReportElementType Type { get; set; }
        public bool IsCorrect { get; set; } = true;

        public MeasurementType MeasurementType { get; set; }
        public string MeasurementName { get; set; }

        public List<dynamic> Values { get; set; }

		public int X { get; set; }
		public int Y { get; set; }
		public int Weight { get; set; }
		public int Height { get; set; }
	}
}
