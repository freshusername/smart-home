using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace smart_home_web.Models
{
    public class ScheduleViewModel
    {
        public List<DateTimeOffset> Dates { get; set; }
        public List<dynamic> Values { get; set; }
    }
}
