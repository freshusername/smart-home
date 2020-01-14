using Domain.Core.Model.Enums;
using System.Collections.Generic;
using System;

namespace Infrastructure.Business.DTOs.ReportElements
{
    public class ColumnRangeDTO
    {
        public int SensorId { get; set; }
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
