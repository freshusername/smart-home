using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Core.Model
{
    public class Control
    {
        public int Id { get; set; }
        public Guid Token { get; set; }

        public virtual ICollection<SensorControl> SensorControls { get; set; }
    }
}
