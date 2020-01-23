using Domain.Core.Model;
using Domain.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Data.Repositories
{
    public class NotificationRepo : BaseRepository<Notification>, INotificationRepo
    {
        public NotificationRepo(ApplicationsDbContext context) : base(context)
        {

        }

        public override async Task<IEnumerable<Notification>> GetAll()
        {
            var res = await context.Notifications
                            .Include(h => h.Sensor)
                            .ThenInclude(s => s.SensorType).ToListAsync();

            return res;
        }

        public override async Task<Notification> GetById(int id)
        {
            return await context.Notifications
                            .FirstOrDefaultAsync(s => s.Id == id);
        }

        public async Task<IEnumerable<Notification>> GetBySensorId(int sensorId)
        {
            return await context.Notifications
                            .Where(s => s.SensorId == sensorId).ToListAsync();
        }
    }
}
