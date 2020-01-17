using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Core.Filters;
using Domain.Core.Model;
using Domain.Core.Model.Enums;
using Domain.Interfaces;
using Domain.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;
using MySql.Data.MySqlClient;

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

        public async Task<int> GetAmountAsync(bool isActivated)
        {
            return await context.Histories.Where(p => p.Sensor.IsActivated == isActivated).CountAsync();
        }

        public async Task<IEnumerable<History>> GetHistoriesBySensorId(int SensorId)
        {
            var histories = context.Histories.Include(h => h.Sensor)
                .ThenInclude(st => st.SensorType)
                .Where(h => h.Sensor.Id == SensorId);

            return await histories.ToListAsync();
        }

        public async Task<IEnumerable<History>> GetHistoriesBySensorIdAndDatePeriod
            (int sensorId, DateTime dateFrom, DateTime dateTo)
        {
            //MySqlParameter p0 = new MySqlParameter("p0", sensorId);
            //MySqlParameter p1 = new MySqlParameter("p1", dateFrom);
            //MySqlParameter p2 = new MySqlParameter("p2", dateTo);

            var d_from = dateFrom.ToString("yyyy-MM-dd HH:mm");
            var d_to = dateTo.ToString("yyyy-MM-dd HH:mm");

            var histories = context.Histories.FromSql($"CALL SensorValuesForTimePeriod ({0}, '{1}', '{2}')", sensorId, d_from, d_to);

            return await histories.ToListAsync();
        }

        //public async Task<IEnumerable<History>> GetHistoriesBySensorIdAndDatePeriod
        //    (int sensorId, DateTimeOffset dateFrom, DateTimeOffset dateTo)
        //{
        //    var histories = context.Histories.Include(h => h.Sensor)
        //        .ThenInclude(st => st.SensorType)
        //        .Where(h => h.Sensor.Id == sensorId && h.Date > dateFrom && h.Date < dateTo);

        //    return await histories.ToListAsync();
        //}


        public async Task<IEnumerable<History>> GetHistoriesBySensorIdAndDate(int SensorId, DateTimeOffset date)
        {
            var histories = context.Histories.Include(h => h.Sensor)
                .ThenInclude(st => st.SensorType)
                .Where(h => h.Sensor.Id == SensorId && h.Date > date);

            return await histories.ToListAsync();
        }


        public async Task<IEnumerable<History>> GetByPage(int count, int page, SortState sortState, bool isActivated = true, int sensorId = 0)
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

        //public async Task<double?> GetMinValueAfterDate(int SensorId, DateTimeOffset dateTime)
        //{
        //	var histories = await GetHistoriesBySensorId(SensorId);
        //	double? minvalue = null;
        //	if (histories.Any())
        //	{
        //		minvalue = histories
        //						.Where(h => h.Date > dateTime)
        //						.Min(h => (h.DoubleValue.HasValue ? h.DoubleValue : h.IntValue));
        //	}
        //	return minvalue;
        //}

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
    }
}
