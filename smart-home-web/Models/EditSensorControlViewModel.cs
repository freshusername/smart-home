using Domain.Core.Model;
using Domain.Core.Model.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace smart_home_web.Models
{
    public class EditSensorControlViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public int ControlId { get; set; }

        public int Icon { get; set; }

        public int SensorId { get; set; }
      
    }
}
