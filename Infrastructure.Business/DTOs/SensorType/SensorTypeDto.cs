using Domain.Core.Model.Enums;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Business.DTOs.SensorType
{
    public class SensorTypeDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Comment { get; set; }
        public MeasurmentType MeasurementType { get; set; }
        public string MeasurementName { get; set; }
        public int? IconId { get; set; }
        public string IconPath { get; set; }


    }
}
