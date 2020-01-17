using Domain.Core.Model.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace smart_home_web.Models.ReportElements
{
    public class GaugeUpdateViewModel
    {
        public ReportElementHours Hours { get; set; }
        public bool IsValid { get; set; }
        public double? Value { get; set; }
        public int Min { get; set; }
        public int Max { get; set; }
    }
}
