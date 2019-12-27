using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace smart_home_web.Models.SensorViewModel
{
    public class ListSensorViewModel
    {
        public IEnumerable<SensorViewModel> Sensors { get; set; }
    }
}
