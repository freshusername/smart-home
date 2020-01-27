using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Core.CalculateModel;
using Domain.Core.Filters;
using Domain.Core.Model;
using Domain.Core.Model.Enums;
using Domain.Interfaces.Repositories;
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
            return await context.Histories
                .FirstOrDefaultAsync(s => s.Id == id);
        }

        public async Task<History> GetLastBySensorId(int sensorId)
        {
            var history = await context.Histories
                .LastOrDefaultAsync(h => h.SensorId == sensorId);
            return history;
        }

        public async Task<int> GetAmountAsync(bool isActivated)
        {
            return await context.Histories.Where(p => p.Sensor.IsActivated == isActivated).CountAsync();
        }

		public async Task<int> GetAmountAsync(bool isActivated, string userId)
		{
			return await context.Histories.Where(p => p.Sensor.IsActivated == isActivated & p.Sensor.AppUserId == userId).CountAsync();
		}

		public async Task<IEnumerable<History>> GetHistoriesBySensorId(int SensorId)
        {
            var histories = context.Histories.Include(h => h.Sensor)
                .ThenInclude(st => st.SensorType)
                .Where(h => h.Sensor.Id == SensorId);

            return await histories.ToListAsync();
        }

        public async Task<IEnumerable<AvgSensorValuePerDay>> GetAvgSensorsValuesPerDays
            (int sensorId, DateTime dateFrom, DateTime dateTo)
        {
            var d_from = dateFrom.ToString("yyyy-MM-dd");
            var d_to = dateTo.ToString("yyyy-MM-dd");

            string query = $"CALL GetAvgValuesForSensor ({sensorId}, '{d_from}', '{d_to}')";
            var avgValues = await context.AvgSensorValuesPerDays
				.FromSql(query)
				.ToListAsync();


            return avgValues;
        }

        public Task<IEnumerable<History>> GetHistoriesBySensorIdAndDatePeriod(int SensorId, DateTime dateFrom, DateTime dateTo)
        {
            throw new NotImplementedException();
        }
        public async Task<IEnumerable<History>> GetHistoriesBySensorIdAndDatePeriod
            (int sensorId, DateTimeOffset dateFrom, DateTimeOffset dateTo)
        {
            var histories = context.Histories.Include(h => h.Sensor)
                .ThenInclude(st => st.SensorType)
                .Where(h => h.Sensor.Id == sensorId && h.Date > dateFrom && h.Date < dateTo);

            return await histories.ToListAsync();
        }

        public async Task<IEnumerable<History>> GetHistoriesBySensorIdAndDate(int SensorId, DateTimeOffset date)
        {
            var histories = context.Histories.Include(h => h.Sensor)
                .ThenInclude(st => st.SensorType)
                .Where(h => h.Sensor.Id == SensorId && h.Date > date);

            return await histories.ToListAsync();
        }

        public History GetLastHistoryBySensorIdAndDate(int sensorId, DateTimeOffset date)
        {
            var histories = context.Histories.Where(h => h.SensorId == sensorId && h.Date > date);
            if (histories == null) return null;

            var lasthistory = histories.OrderByDescending(e => e.Date).FirstOrDefault();
                 
            return lasthistory;
        }

        public async Task<IEnumerable<History>> GetByPage(int count, int page, SortState sortState, bool isActivated,  int sensorId = 0)
        {
            IQueryable<History> histories = context.Histories
                .Include(h => h.Sensor)
                .ThenInclude(s => s.SensorType);

            if (!isActivated)
                histories = histories.Where(p => p.Sensor.IsActivated == false);


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

        public History GetLastHistoryBySensorId(int SensorId)
        {
            var histories = context.Histories.Include(h => h.Sensor)
                                                .ThenInclude(st => st.SensorType)
                                            .Where(h => h.Sensor.Id == SensorId)
                                            .OrderBy(h => h.Date)
                                            .Last();
            return histories;
        }

        public double? GetMinValueForPeriod(int sensorId, int? hours)
        {
            var items = context.Histories.Where(h => h.SensorId == sensorId);

            if (hours.HasValue && hours != 0)
            {
                items = items.Where(h => h.Date > (DateTimeOffset.Now - new TimeSpan(hours.Value, 0, 0)));
            }

            double? minvalue = null;
            try
            {
                minvalue = items.Min(h => (h.DoubleValue.HasValue ? h.DoubleValue : h.IntValue));
            }
            catch
            {

            }

            return minvalue;
        }

        public double? GetMaxValueForPeriod(int sensorId, int? hours)
        {
            var items = context.Histories.Where(h => h.SensorId == sensorId);

            if (hours.HasValue && hours != 0)
            {
                items = items.Where(h => h.Date > (DateTimeOffset.Now - new TimeSpan(hours.Value, 0, 0)));
            }

            double? maxvalue = null;
            try
            {
                maxvalue = items.Max(h => (h.DoubleValue.HasValue ? h.DoubleValue : h.IntValue));
            }
            catch
            {

            }

            return maxvalue;
        }


        public int? GetIntMinValueForPeriod(int sensorId, int? minutes)
        {
            var items = context.Histories.Where(h => h.SensorId == sensorId);

            if (minutes.HasValue && minutes != 0)
            {
                items = items.Where(h => h.Date > (DateTimeOffset.Now - new TimeSpan(0, minutes.Value, 0)));
            }

            int? minvalue = null;
            try
            {
                minvalue = items.Min(h => h.IntValue);
            }
            catch
            {

            }

            return minvalue;
        }

        public int? GetIntMaxValueForPeriod(int sensorId, int? minutes)
        {
            var items = context.Histories.Where(h => h.SensorId == sensorId);

            if (minutes.HasValue && minutes != 0)
            {
                items = items.Where(h => h.Date > (DateTimeOffset.Now - new TimeSpan(0, minutes.Value, 0)));
            }

            int? maxvalue = null;
            try
            {
                maxvalue = items.Max(h => h.IntValue);
            }
            catch
            {

            }

            return maxvalue;
        }       
    }
}
