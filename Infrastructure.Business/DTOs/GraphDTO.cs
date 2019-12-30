using Domain.Core.Model.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Business.DTOs
{
    public class GraphDTO
    {
        public int SensorId { get; set; }
        public string SensorName { get; set; }
        public string SensorType { get; set; }
        public bool IsCorrect { get; set; }

        public MeasurmentType MeasurmentType { get; set; } 
        public string MeasurmentName { get; set; } 

        public List<DateTimeOffset> Dates { get; set; }

        public List<string> StringValues { get; set; }
        public List<int> IntValues { get; set; }
        public List<double> DoubleValues { get; set; }
        public List<bool> BoolValues { get; set; }
    }
}
