using System;
using System.Collections.Generic;
using System.Text;
using Domain.Core.Model;

namespace Infrastructure.Business.DTOs.Sensor
{
	public class SensorDto
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public string Comment { get; set; }
		public byte[] Icon { get; set; }
		public Guid Token { get; set; }
		public DateTimeOffset? CreatedOn { get; set; }
		public bool IsActivated { get; set; }


		public int IconId { get; set; }
		public int SensorTypeId { get; set; }
        public virtual ICollection<SensorType> SensorTypes { get; set; }
        public virtual ICollection<Domain.Core.Model.History> Histories { get; set; }
	}
}
