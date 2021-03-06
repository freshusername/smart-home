﻿using Domain.Core.Model.Enums;
using System;
using System.Collections.Generic;

namespace Infrastructure.Business.DTOs
{
	public class GraphDto
    {
        public int SensorId { get; set; }
        public string SensorName { get; set; }
        public string SensorType { get; set; }
        public bool IsCorrect { get; set; } = true;

        public MeasurementType? MeasurementType { get; set; } 
        public string MeasurementName { get; set; } 

        public List<DateTimeOffset> Dates { get; set; }

        public List<dynamic> Values { get; set; }
    }
}
