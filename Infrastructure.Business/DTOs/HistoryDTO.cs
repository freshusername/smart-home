using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Business.DTOs
{
    public class HistoryDTO
    {
        public int Id { get; set; }
        public DateTimeOffset Date { get; set; }

        public string StringValue { get; set; }
        public int IntValue { get; set; }
        public double DoubleValue { get; set; }
        public bool BoolValue { get; set; }

        public int SensorId { get; set; }
    }
}
