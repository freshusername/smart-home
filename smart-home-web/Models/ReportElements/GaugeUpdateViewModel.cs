﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace smart_home_web.Models.ReportElements
{
    public class GaugeUpdateViewModel
    {
        public double? Value { get; set; }
        public int Min { get; set; }
        public int Max { get; set; }
    }
}
