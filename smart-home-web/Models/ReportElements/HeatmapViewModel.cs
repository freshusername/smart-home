﻿using Domain.Core.CalculateModel;
using Domain.Core.Model.Enums;
using System.Collections.Generic;

namespace smart_home_web.Models.ReportElements
{
	public class HeatmapViewModel
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

        public MeasurementType MeasurementType { get; set; }
        public string MeasurementName { get; set; }

        public List<string> Dates { get; set; }
        public List<AvgSensorValuePerDay> AvgSensorValuesPerDays { get; set; }

    }
}
