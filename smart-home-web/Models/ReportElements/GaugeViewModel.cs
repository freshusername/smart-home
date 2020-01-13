﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace smart_home_web.Models.ReportElements
{
    public class GaugeViewModel
    {
        public string SensorName { get; set; }
        public string MeasurementName { get; set; }
        public bool IsValid { get; set; }
        public int Min { get; set; }
        public int Max { get; set; }
    }
}