using Domain.Core.Model.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace smart_home_web.Models.ReportElements
{
    public class ReportElementViewModel
    {
        public int Id { get; set; }
        public int DashboardId { get; set; }
        public string DashboardName { get; set; }
        public int SensorId { get; set; }
        public string SensorName { get; set; }
        public int Hours { get; set; }
        public bool IsCorrect { get; set; }

        public ReportElementType Type { get; set; }
        
        public MeasurementType MeasurementType { get; set; }
        public string MeasurementName { get; set; }

        public List<dynamic> Values { get; set; }

        public List<string> Dates { get; set; }
        public List<int?> MinValues { get; set; }
        public List<int?> MaxValues { get; set; }
    }
}

