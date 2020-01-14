using Infrastructure.Business.DTOs.Options;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Business.DTOs.DashboardOptions
{
	public class DashboardOptionsDto
	{
		public int Id { get; set; }
		public int DashboardId { get; set; }
		public List<OptionsDto> Options { get; set; }
	}
}
