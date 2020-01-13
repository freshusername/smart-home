using Domain.Core.Model.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Business.DTOs.ReportElements
{
    class ReportElementDto
    {
        public int Id { get; set; }
        public int DashboardId { get; set; }
        public string DashboardName { get; set; }
        public int SensorId { get; set; }
        public string SensorName { get; set; }
        public int Hours { get; set; }
        public ReportElementType Type { get; set; }
        public MeasurementType MeasurementType { get; set; }
        public string MeasurementName { get; set; }

        public List<string> StringValues { get; set; }
        public List<int> IntValues { get; set; }
        public List<double> DoubleValues { get; set; }
        public List<bool> BoolValues { get; set; }
    }
}
