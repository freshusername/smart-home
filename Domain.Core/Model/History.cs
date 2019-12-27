using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Core.Model
{
    public class History
    {
        public int Id { get; set; }
        public DateTimeOffset Date { get; set; }

        public string StringValue { get; set; } = null;
        public int? IntValue { get; set; } = null;
		public double? DoubleValue { get; set; } = null;
		public bool? BoolValue { get; set; } = null;

        public int SensorId { get; set; }		
        public Sensor Sensor { get; set; }

        public Message Message { get; set; }
    }
}
