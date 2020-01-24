using Domain.Core.Model;
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
	public class DashboardRepo : BaseRepository<Dashboard>, IDashboardRepo
	{
		public DashboardRepo(ApplicationsDbContext context) : base(context)
		{

		}

		public override async Task<IEnumerable<Dashboard>> GetAll()
		{
			return await context.Dashboards
				.Include(d => d.AppUser)
				.Include(d => d.ReportElements).ToListAsync();
		}

		public override async Task DeleteById(int id)
		{
			context.Dashboards.Remove(await GetById(id));
		}

		public override async Task<Dashboard> GetById(int id)
		{
			return await context.Dashboards
				.Include(d => d.AppUser)
				.Include(d => d.ReportElements)
				.FirstOrDefaultAsync(s => s.Id == id);
		}
	}
}
