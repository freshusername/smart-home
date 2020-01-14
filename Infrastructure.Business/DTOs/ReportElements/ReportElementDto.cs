using Domain.Core.Model.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Business.DTOs.ReportElements
{
    public class ReportElementDto
    {
        public int Id { get; set; }
        public int DashboardId { get; set; }
        public string DashboardName { get; set; }
        public int SensorId { get; set; }
        public string SensorName { get; set; }
        public int Days { get; set; }

        public IEnumerable<DateTimeOffset> Dates { get; set; }
        public IEnumerable<dynamic> Values { get; set; }
    }
}
