using Domain.Core.JoinModel;
using Domain.Interfaces;
using Domain.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Data.Repositories
{
	public class DashboardOptionsRepo : BaseRepository<DashboardOptions>, IDashboardOptionsRepo
	{
		public DashboardOptionsRepo(ApplicationsDbContext context) : base(context)
		{

		}

		public async Task<DashboardOptions> GetByDashboardId(int id)
		{
			return context.DashboardOptions
				.Include(d => d.Dashboard)
				.Include(d => d.Options)
				.FirstOrDefault(d => d.DashboardId == id);
		}
	}
}
