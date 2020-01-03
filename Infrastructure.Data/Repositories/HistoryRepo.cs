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
		

		public HistoryRepo(ApplicationsDbContext _context) : base(_context)
		{
		
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
			return _context.Histories
				.FirstOrDefault(s => s.Id == id);
		}

		public IEnumerable<History> GetHistoriesBySensorId(int SensorId)
		{
            var histories = _context.Histories.Include(h => h.Sensor)
                                            .ThenInclude(st => st.SensorType)
                                            .Select(h => h)
                                            .Where(h => h.Sensor.Id == SensorId);
            return histories;
        }
    }
}
