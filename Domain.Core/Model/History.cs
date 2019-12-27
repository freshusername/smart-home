using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Core.Model
{
    public class History
    {
        public int Id { get; set; }
        public DateTimeOffset Date { get; set; }

        public string StringValue { get; set; }
        public int IntValue { get; set; }
        public double DoubleValue { get; set; }
        public bool BoolValue { get; set; }

        public Sensor Sensor { get; set; }
        public Message Message { get; set; }
    }
}
