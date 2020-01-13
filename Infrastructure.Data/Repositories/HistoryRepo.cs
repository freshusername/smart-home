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
		

		public HistoryRepo(ApplicationsDbContext context) : base(context)
		{
		
		}

		public override async Task<IEnumerable<History>> GetAll()
		{
			var res = await context.Histories
				.Include(h => h.Sensor)
				.ThenInclude(s => s.SensorType).ToListAsync();

			return res;
		}

		public override async Task<History> GetById(int id)
		{
			return context.Histories
				.FirstOrDefault(s => s.Id == id);
		}

		public IEnumerable<History> GetHistoriesBySensorId(int SensorId)
		{
            var histories = context.Histories.Include(h => h.Sensor)
												.ThenInclude(st => st.SensorType)
                                            .Select(h => h)
                                            .Where(h => h.Sensor.Id == SensorId);
            return histories;
        }

		public History GetLastHistoryBySensorId(int SensorId)
		{
			var histories = context.Histories.Include(h => h.Sensor)
												.ThenInclude(st => st.SensorType)
											.Select(h => h)
											.Where(h => h.Sensor.Id == SensorId)
											.OrderBy(h => h.Date)
											.Last();
			return histories;
		}

		public double? GetMinValueAfterDate(int SensorId, DateTimeOffset dateTime)
		{
			var histories = GetHistoriesBySensorId(SensorId);
			double? minvalue = null;
			if (histories.Any())
			{ 
				minvalue = histories
								//.Where(h => h.Date > dateTime)
								.Min(h => (h.DoubleValue.HasValue ? h.DoubleValue : h.IntValue));
			}
			return minvalue;
		}

		public double? GetMaxValueAfterDate(int SensorId, DateTimeOffset dateTime)
		{
			var histories = GetHistoriesBySensorId(SensorId);
			double? maxvalue = null;
			if (histories.Any())
			{
				maxvalue = histories
								//.Where(h => h.Date > dateTime)
								.Max(h => (h.DoubleValue.HasValue ? h.DoubleValue : h.IntValue));
			}
			return maxvalue;
		}
	}
}
