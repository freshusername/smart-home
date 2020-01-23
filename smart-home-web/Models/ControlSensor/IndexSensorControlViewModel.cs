using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace smart_home_web.Models.ControlSensor
{
    public class IndexSensorControlViewModel
    {
        public EditSensorControlViewModel EditViewModel { get; set; }
        public SensorControlViewModel SensorControl { get; set; }

        public List<SensorViewModel.SensorViewModel> ControlSensorsView { get; set; }
        public List<SensorViewModel.SensorViewModel> SensorsView { get; set; }


    }
}
