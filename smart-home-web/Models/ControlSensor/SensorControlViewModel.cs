using Domain.Core.Model;
using Domain.Core.Model.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace smart_home_web.Models.ControlSensor
{
    public class SensorControlViewModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public bool IsActive { get; set; } = false;

       
    }
}
