using Domain.Core.Model.Enums;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace smart_home_web.Models.SensorType
{
    public class CreateSensorTypeViewModel
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public string Comment { get; set; }

        [Required]
        [DisplayName("Measurment Type")]
        public MeasurmentType MeasurmentType { get; set; }

        [Required]
        [DisplayName("Measurment Name")]
        public string MeasurmentName { get; set; }

        public IFormFile Icon { get; set; }
    }
}
