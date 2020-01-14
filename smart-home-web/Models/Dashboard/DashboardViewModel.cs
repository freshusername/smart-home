using Domain.Core.JoinModel;
using Domain.Core.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Core.JoinModel;

namespace smart_home_web.Models.Dashboard
{
	public class DashboardViewModel
	{
		public int Id { get; set; }
		public string Name { get; set; }
		
		public ICollection<Domain.Core.JoinModel.DashboardOptions> DashboardOptions { get; set; }
		public ICollection<ReportElement> ReportElements { get; set; }
	}
}
