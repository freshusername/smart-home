﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Business.DTOs.ReportElements
{
    public class GaugeDto
    {
        public int Id { get; set; }
        public int DashboardId { get; set; }
        public int SensorId { get; set; }
        public bool IsValid { get; set; } = false;

        public string SensorName { get; set; }
        public string MeasurementName { get; set; }
        public double? Min { get; set; }
        public double? Max { get; set; }
    }
}