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

        public ActionRole Role { get; set; }

        public bool IsActive { get; set; } = false;

        public Control Control { get; set; }

        public Domain.Core.Model.Sensor Sensor { get; set; }
        public Domain.Core.Model.Sensor ControlSensor { get; set; }
    }
}
