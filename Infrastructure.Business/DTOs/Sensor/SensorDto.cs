using System;

namespace Infrastructure.Business.DTOs.Sensor
{
	public class SensorDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Comment { get; set; }
        public Guid Token { get; set; }
        public int? IconId { get; set; }
        public int SensorTypeId { get; set; }
        public string AppUserId { get; set; }
        public DateTimeOffset? CreatedOn { get; set; }
        public bool IsActivated { get; set; }
        public string IconPath { get; set; }
        public string SensorTypeName { get; set; }
        public bool IsActive { get; set; }
    }
}
