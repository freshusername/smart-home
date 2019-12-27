using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

		public override IEnumerable<History> GetAll()
		{
			var res = _context.Histories
				.Include(h => h.Sensor)
				.ThenInclude(s => s.SensorType);

			return res;
		}

		public override History GetById(int id)
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
