﻿using Domain.Core.Model.Enums;
using System.Collections.Generic;

namespace Domain.Core.Model
{
	public class SensorType
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Comment { get; set; }
        public MeasurementType MeasurementType { get; set; }
        public string MeasurementName { get; set; }
		//TODO: Replace to Centrols
		public bool IsControl { get; set; }
        
        public int? IconId { get; set; }
        public Icon Icon { get; set; }

        public virtual ICollection<Sensor> Sensor { get; set; }
    }
}