using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Business.DTOs.Options
{
	public class OptionsDto
	{
		public int Id { get; set; }

		public int X { get; set; }
		public int Y { get; set; }
		public int Weight { get; set; }
		public int Height { get; set; }

		public int DashboardOptionsId { get; set; }
	}
}
