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

		public virtual ICollection<Domain.Core.Model.SensorType> SensorTypes { get; set; }
		public virtual ICollection<Domain.Core.Model.History> Histories { get; set; }
	}
}
