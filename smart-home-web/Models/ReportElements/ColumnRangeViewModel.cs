﻿using Domain.Core.Model.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace smart_home_web.Models.ReportElements
{
    public class ColumnRangeViewModel
    {
        public int SensorId { get; set; }
        public string SensorName { get; set; }
        public string SensorType { get; set; }
        public bool IsCorrect { get; set; }
        public int Days { get; set; }
        public string DashboardName { get; set; }

        public List<long> longDates { get; set; }
        public List<int> MaxValues { get; set; }
        public List<int> MinValues { get; set; }
    }
}