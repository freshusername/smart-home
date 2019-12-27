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
		

		public HistoryRepo(ApplicationsDbContext _context) : base(_context)
		{
		
		}

		public override IEnumerable<History> GetAll()
		{
			var res = _context.Histories
				.Include(h => h.Sensor);
			return res;
		}

		public override History GetById(int id)
		{
			return _context.Histories.Include(h => h.Sensor).FirstOrDefault(s => s.Id == id);
		}

		public IEnumerable<History> GetHistoriesBySensorId(int SensorId)
		{
			throw new NotImplementedException();
		}
    }
}
