using Domain.Core.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Business.DTOs.Dashboard
{
	public class DashboardDto
	{
		public int Id { get; set; }
		public string Name { get; set; }

		public string AppUserId { get; set; }
		public AppUser AppUser { get; set; }

		public ICollection<ReportElement> ReportElements { get; set; }
	}
}
