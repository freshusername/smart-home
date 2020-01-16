﻿using Domain.Core.Model.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace smart_home_web.Models.ReportElements
{
    public class CreateReportElementViewModel
    {
        public int DashboardId { get; set; }
        public int SensorId { get; set; }
        public ReportElementType Type { get; set; }
        public int Hours { get; set; }
    }
}
