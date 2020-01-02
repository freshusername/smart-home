using Domain.Core.Model.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Business.Infrastructure
{
    public static class MeasureType<T>
    {
        public static MeasurmentType? GetMeasureType(T value)
        {
            
            switch (Type.GetTypeCode(value.GetType()))
            {
                case TypeCode.Int32:
                    return MeasurmentType.Int;
                case TypeCode.Double:
                    return MeasurmentType.Double;
                case TypeCode.String:
                    return MeasurmentType.String;
                case TypeCode.Boolean:
                    return MeasurmentType.Bool;
                default:
                    return null;
            }
        }
    }
}
