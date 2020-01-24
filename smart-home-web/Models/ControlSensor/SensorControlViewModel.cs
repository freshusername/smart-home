using Domain.Core.Model;
using Domain.Core.Model.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace smart_home_web.Models.ControlSensor
{
    public class SensorControlViewModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public bool IsActive { get; set; } = false;

        public Icon Icon { get; set; }

        public Sensor Sensor { get; set; }

        public int SensorId { get; set; }
        public int ControlId { get; set; }

        public Sensor ControlSensor { get; set; }       
    }
}
