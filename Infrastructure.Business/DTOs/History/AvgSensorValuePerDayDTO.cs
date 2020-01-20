using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Business.DTOs.History
{
    public class AvgSensorValuePerDayDTO
    {
        public DateTime WeekDay { get; set; }
        public double AvgValue { get; set; }
    }
}
