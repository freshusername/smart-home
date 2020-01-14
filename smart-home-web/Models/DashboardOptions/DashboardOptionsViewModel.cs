using smart_home_web.Models.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace smart_home_web.Models.DashboardOptions
{
	public class DashboardOptionsViewModel
	{
		public int Id { get; set; }
		public int DashboardId { get; set; }
		public List<OptionsViewModel> OptionsId { get; set; }
	}
}
