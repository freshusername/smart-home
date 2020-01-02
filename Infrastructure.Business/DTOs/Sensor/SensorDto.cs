using System;
using System.Collections.Generic;
using System.Text;
using Domain.Core.Model;
using Domain.Core.Model.Enums;

namespace Infrastructure.Business.DTOs.Sensor
{
    public class SensorDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Comment { get; set; }
        public Guid Token { get; set; }
        public DateTimeOffset? CreatedOn { get; set; }
        public bool IsActivatedOn { get; set; }

    }
}
