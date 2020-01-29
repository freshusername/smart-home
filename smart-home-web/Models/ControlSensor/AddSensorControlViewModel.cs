using Domain.Core.Model;
using Domain.Core.Model.Enums;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace smart_home_web.Models.ControlSensor
{
    public class AddSensorControlViewModel
    {

        [Required]
        [StringLength(20, ErrorMessage = "The Name must be at least 2 and at max 20 characters long.", MinimumLength = 2)]
        public string Name { get; set; }

        [Required]
        public int? ControlId { get; set; }

        [Required]
        public int? SensorId { get; set; }

        public int? maxValue { get; set; }

        public int? minValue { get; set; }

        public int? IconId { get; set; }
        public string IconPath { get; set; }

        [DisplayName("Icon")]
        public IFormFile IconFile { get; set; }

        public bool IsActive { get; set; } = true;

    }
}
