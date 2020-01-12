using Domain.Core.Model.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace smart_home_web.Models
{
    public class GraphViewModel
    {
        public int SensorId { get; set; }
        public string SensorName { get; set; }
        public string SensorType { get; set; }
        public bool IsCorrect { get; set; }
        public int Days { get; set; }
        public string DashboardName { get; set; }

        public MeasurementType MeasurementType { get; set; }
        public string MeasurementName { get; set; }

        public List<long> longDates { get; set; }

        public List<string> StringValues { get; set; }
        public List<int> IntValues { get; set; }
        public List<double> DoubleValues { get; set; }
        public List<bool> BoolValues { get; set; }
    }
}