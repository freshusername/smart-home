using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Core.Model
{
    public class History
    {
        public int Id { get; set; }
        public DateTimeOffset Date { get; set; }
        public string Value { get; set; } // TODO 4 history 

        public int SensorId { get; set; }
        public virtual Sensor Sensor { get; set; }

        public virtual Message Message { get; set; }
    }
}
