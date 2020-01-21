using Domain.Core.Model;
using Domain.Core.Model.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace smart_home_web.Models
{
    public class SensorControlViewModel
    {
        public int Id { get; set; }

        public ActionRole Role { get; set; }

        public bool IsActive { get; set; } = false;      

        public Control Control { get; set; }

        public Sensor Sensors { get; set; }
    }
}
