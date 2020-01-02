using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Business.Infrastructure
{
    class ValueParser 
    {
        public ValuesModel Values;

        public ValuesModel Parse (string value)
        {
            if (int.TryParse(value, out Values.IntValue))
                return Values;
            if (double.TryParse(value, out Values.DoubleValue))
                return Values;
            if (bool.TryParse(value, out Values.BoolValue))
                return Values;

            Values.StringValue = value;
           
            return Values;
        }
    }
}
