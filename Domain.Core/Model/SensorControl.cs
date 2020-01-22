using Domain.Core.Model.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Core.Model
{
    public class SensorControl
    {
        public int Id { get; set; }

        public CheckBy Rule { get; set; }
        public int Time { get; set; }
        public bool IsActive { get; set; } = false;
       

        public int SensorId { get; set; }
        public virtual Sensor Sensor { get; set; }

        public int ControlId { get; set; }
        public virtual Control Control { get; set; }

        public int IconId { get; set; }
        public Icon Icon { get; set; }


        
    }
}
