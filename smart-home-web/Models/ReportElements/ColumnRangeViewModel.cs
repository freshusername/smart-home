using Domain.Core.Model.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace smart_home_web.Models.ReportElements
{
    public class ColumnRangeViewModel
    {
        public string SensorName { get; set; }
        public bool IsCorrect { get; set; }
        public int Hours { get; set; }
        public string DashboardName { get; set; }
        public ReportElementType Type { get; set; }

        public List<DateTimeOffset> Dates { get; set; }
        public List<int?> MaxValues { get; set; }
        public List<int?> MinValues { get; set; }
    }
}
