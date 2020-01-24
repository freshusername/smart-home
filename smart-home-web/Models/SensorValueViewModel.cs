using System;

namespace smart_home_web.Models
{
	public class SensorValueViewModel<T>
    {
        public Guid Token { get; set; }
        public T Value { get; set; }
    }
}
