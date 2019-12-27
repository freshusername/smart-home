using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace smart_home_web.Models.History
{
    public enum HistorySortState
    {
        SensorAsc,
        SensorDesc,
        DateAsc,
        DateDesc,
        StringValueAsc,
        StringValueDesc,
        IntValueAsc,
        IntValueDesc,
        DoubleValueAsc,
        DoubleValueDesc,
        BoolValueAsc,
        BoolValueDesc
    }
}
