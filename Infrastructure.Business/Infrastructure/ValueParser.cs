using Domain.Core.Model.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Business.Infrastructure
{
    public static class ValueParser
    {
        public static int IntValue;
        public static double DoubleValue;
        public static bool BoolValue;
     
        public static dynamic Parse(string value)
        {
               
            if (int.TryParse(value, out IntValue))
                return IntValue;
            if (double.TryParse(value, out DoubleValue))
                return DoubleValue;
            if (bool.TryParse(value, out BoolValue))
                return BoolValue;

            return value;                     
        }

        public static dynamic Parse(string value, MeasurementType? type)
        {
            switch (type)
            {
                case MeasurementType.Int:
                  int.TryParse(value, out IntValue);
                 return IntValue;

                case MeasurementType.Double:
                  double.TryParse(value, out DoubleValue);
                 return DoubleValue;

                case MeasurementType.Bool:
                  BoolValue = Convert.ToBoolean(Convert.ToInt16(value));
                 return BoolValue;

                default: return value;
            }
         
        }
    }
}
