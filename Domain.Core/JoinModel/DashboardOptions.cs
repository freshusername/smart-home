using Domain.Core.Model;
using System.Collections.Generic;

namespace Domain.Core.JoinModel
{
	//TODO: Ensure that such model can be placed in this project
	public class DashboardOptions
	{
		public int Id { get; set; }

		public int DashboardId { get; set; }
		public Dashboard Dashboard { get; set; }

		public int OptionsId { get; set; }
		public List<Options> Options { get; set;}
	}
}
