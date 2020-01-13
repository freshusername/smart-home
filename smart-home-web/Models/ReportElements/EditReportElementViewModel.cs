using Domain.Core.Model.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace smart_home_web.Models.ReportElements
{
    public class EditReportElementViewModel
    {
        public int Id { get; set; }
        public int DashboardId { get; set; }
        public string DashboardName { get; set; }
        public int SensorId { get; set; }
        public string SensorName { get; set; }
        public double Hours { get; set; }

        public ReportElementType Type { get; set; }
    }
}
