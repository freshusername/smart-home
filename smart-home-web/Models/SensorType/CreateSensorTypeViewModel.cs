using Domain.Core.Model.Enums;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace smart_home_web.Models.SensorType
{
    public class CreateSensorTypeViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Comment { get; set; }
        public MeasurmentType MeasurmentType { get; set; }
        public string MeasurmentName { get; set; }
        public IFormFile Icon { get; set; }
    }
}
