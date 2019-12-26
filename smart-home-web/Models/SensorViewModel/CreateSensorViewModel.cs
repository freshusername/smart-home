using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace smart_home_web.Models.SensorViewModel
{
    public class CreateSensorViewModel
    {
        public string Name { get; set; }
        public string Comment { get; set; }
        //public byte[] Icon { get; set; }
        public IFormFile Icon { get; set; }
        public Guid Token { get; set; }
        //public int SensorTypeId { get; set; }
    }
}
