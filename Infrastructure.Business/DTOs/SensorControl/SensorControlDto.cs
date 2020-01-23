using Domain.Core.Model;
using Domain.Core.Model.Enums;
using Microsoft.AspNetCore.Http;
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
        
        public int SensorId { get; set; }
        public Domain.Core.Model.Sensor Sensor { get; set; }
        public Domain.Core.Model.Sensor ControlSensor { get; set; }

        public int ControlId { get; set; }
        public Control Control { get; set; }

        public int IconId { get; set; }
        public string IconPath { get; set; }
        public IFormFile IconFile { get; set; }
    }
}
