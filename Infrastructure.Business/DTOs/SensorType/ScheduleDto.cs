using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Business.DTOs.SensorType
{
    public class ScheduleDto
    {
        public List<DateTimeOffset> Dates { get; set; }
        public List<dynamic> Values { get; set; }

    }
}
