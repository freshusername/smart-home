using Domain.Core.Model;
using Domain.Core.Model.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Business.DTOs
{
    public class SensorControlDto
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public bool IsActive { get; set; } = false;

        public int? minValue { get; set; }

        public int? maxValue { get; set; }

        public Domain.Core.Model.Icon Icon { get; set; }
        public Domain.Core.Model.Sensor Sensor { get; set; }
        public Domain.Core.Model.Sensor ControlSensor { get; set; }
        public Control Control { get; set; }
    }
}
