using Domain.Core.Model.Enums;
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
        public Mv Mv { get; set;}
        public string Mn { get; set; }
        public byte[] Icon { get; set; }

        public int SensorId { get; set; }
        public virtual Sensor Sensor{ get; set; }
    }
   
}
