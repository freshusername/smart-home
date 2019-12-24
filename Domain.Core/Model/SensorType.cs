﻿using Domain.Core.Model.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Core.Model
{
    public class SensorType
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Comment { get; set; }
        public MeasurmentType MeasurmentType { get; set; }
        public string MeasurmentName { get; set; }
        public byte[] Icon { get; set; }
		
        public virtual ICollection<Sensor> Sensor{ get; set; }
    }
}