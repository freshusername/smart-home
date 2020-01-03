using Domain.Core.Model;
using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Data.Repositories
{
    public class SensorRepo : BaseRepository<Sensor>, ISensorRepo
    {
        public SensorRepo(ApplicationsDbContext context) : base(context)
        {

        }

        public override async Task<IEnumerable<Sensor>> GetAll()
        {
            var sensors = await context.Sensors
                .Include(s => s.SensorType)
                    .ThenInclude(st => st.Icon)
                .Include(s => s.Icon).ToListAsync();

            return sensors;
        }

        public override async Task<Sensor> GetById(int id)
        {
            return await context.Sensors.FindAsync(id);
        }

        public Sensor GetByToken(Guid token)
        {
            var sensor = _context.Sensors.FirstOrDefault(e => e.Token == token);

            return sensor;
        }

    }
}
