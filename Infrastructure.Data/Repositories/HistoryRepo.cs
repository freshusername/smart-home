using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Core.Model;
using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data.Repositories
{
	public class HistoryRepo : BaseRepository<History>, IHistoryRepo
    {
		private readonly ApplicationsDbContext _context;

		public HistoryRepo(ApplicationsDbContext context) : base(context)
		{
			_context = context;
		}

		public override async Task<IEnumerable<History>> GetAll()
		{
			var res = await _context.Histories
				.Include(h => h.Sensor)
				.ThenInclude(s => s.SensorType).ToListAsync();

			return res;
		}

		public override async Task<History> GetById(int id)
		{
			return await _context.Histories.Include(h => h.Sensor).FirstOrDefaultAsync(s => s.Id == id);
		}

		public IEnumerable<History> GetHistoriesBySensorId(int SensorId)
		{
			throw new NotImplementedException();
		}
    }
}
