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
        [DisplayName("Measurement Type")]
        public MeasurmentType MeasurementType { get; set; }
        [DisplayName("Measurement Name")]
        public string MeasurementName { get; set; }
        public byte[] Icon { get; set; }
        public string IconPath { get; set; }
    }
}
