using Domain.Core.Model.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Core.Model
{
    public class Notification
    {
        public int Id { get; set; }
        public RuleEnum Rule { get; set; }
        public ToastTypes NotificationType { get; set; }
        public string Value { get; set; }
        public string Message { get; set; } = "Sensor $SensorName$ returned value $Value$";

        public int SensorId { get; set; }
        public Sensor Sensor { get; set; }
    }
}
