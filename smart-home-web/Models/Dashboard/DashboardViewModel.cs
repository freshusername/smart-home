
using Domain.Core.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Infrastructure.Business.DTOs.ReportElements;

namespace smart_home_web.Models.Dashboard
{
	public class DashboardViewModel
	{
		public int Id { get; set; }
		public string Name { get; set; }
		
		public ICollection<ReportElementDTO> ReportElements { get; set; }
	}
}
