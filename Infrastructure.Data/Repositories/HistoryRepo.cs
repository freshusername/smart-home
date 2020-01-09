using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Core.Filters;
using Domain.Core.Model;
using Domain.Core.Model.Enums;
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

		public async Task<int> GetAmountAsync()
		{
			return await context.Histories.CountAsync();
		}

		public async Task<IEnumerable<History>> GetHistoriesBySensorId(int SensorId)
		{
			var histories = context.Histories.Include(h => h.Sensor)
				.ThenInclude(st => st.SensorType)
				.Select(h => h)
				.Where(h => h.Sensor.Id == SensorId);

			return await histories.ToListAsync();
		}

		public async Task<IEnumerable<History>> GetByPage(int count, int page, SortState sortState, int sensorId = 0)
		{
			IQueryable<History> histories = context.Histories
				.Include(h => h.Sensor)
				.ThenInclude(s => s.SensorType);

			if (sensorId != 0)
			{
				histories = histories.Where(h => h.Sensor.Id == sensorId);
			}

			var sorted = SortValue.SortHistories(sortState, histories);

			var res = sorted
				.Skip(count * (page - 1))
				 .Take(count);

			return await res.ToListAsync();
		}
	}
}
