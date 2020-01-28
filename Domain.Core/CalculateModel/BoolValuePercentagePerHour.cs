using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Core.CalculateModel
{
    public class BoolValuePercentagePerHour
    {
        public DateTime DayDate { get; set; }
        public int HourTime { get; set; }
        public int? TrueFalseCount { get; set; }
        public int? TrueCount { get; set; }
        public decimal? TruePercentage { get; set; }
    }
}
