using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace smart_home_web.Models.SensorViewModel
{
    public class SensorViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Comment { get; set; }
        public Guid Token { get; set; }
        public DateTimeOffset? ActivatedOn { get; set; }

        public int SensorTypeId { get; set; }
        public int IconId { get; set; }

    }
}
