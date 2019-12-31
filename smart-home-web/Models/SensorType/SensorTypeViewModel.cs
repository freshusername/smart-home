using Domain.Core.Model.Enums;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace smart_home_web.Models.SensorType
{
    public class SensorTypeViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Comment { get; set; }
        [DisplayName("Measurment Type")]
        public MeasurmentType MeasurmentType { get; set; }
        [DisplayName("Measurment Name")]
        public string MeasurmentName { get; set; }
        public byte[] Icon { get; set; }
    }
}
