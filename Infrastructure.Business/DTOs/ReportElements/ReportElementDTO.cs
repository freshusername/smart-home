﻿using Domain.Core.Model.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Business.DTOs.ReportElements
{
    public class ReportElementDto
    {
        public int Id { get; set; }

        public int DashboardId { get; set; }
        public string DashboardName { get; set; }

        public int SensorId { get; set; }
        public string SensorName { get; set; }

        public int Hours { get; set; }
        public string Message { get; set; }
        public ReportElementType Type { get; set; }
        public bool IsCorrect { get; set; } = true;

        public MeasurementType MeasurementType { get; set; }
        public string MeasurementName { get; set; }

        public List<dynamic> Values { get; set; }

        public List<string> Dates { get; set; }
        public List<int?> MinValues { get; set; }
        public List<int?> MaxValues { get; set; }
    }

		public int X { get; set; }
		public int Y { get; set; }
		public int Weight { get; set; }
		public int Height { get; set; }
	}
}
