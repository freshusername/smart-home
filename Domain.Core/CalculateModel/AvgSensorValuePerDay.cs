using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Core.CalculateModel
{
    public class AvgSensorValuePerDay
    {
        public DateTime WeekDay { get; set; }
        public decimal AvgValue { get; set; }
    }
}
