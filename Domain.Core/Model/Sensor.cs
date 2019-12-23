﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Core.Model 
{
    public class Sensor 
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Comment { get; set; }
        public byte[] Icon { get; set; }
        public Guid Token { get; set; }

        public virtual ICollection<SensorType> SensorTypes { get; set; }
        public virtual ICollection<History> Histories { get; set; }
    }
}
