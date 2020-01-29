using Domain.Core.Model;
using Domain.Core.Model.Enums;
using Domain.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Data.Repositories
{
    public class SensorControlRepo : BaseRepository<SensorControl>, ISensorControlRepo
    {
        public SensorControlRepo(ApplicationsDbContext context) : base(context)
        {

        }

        public async Task<IEnumerable<SensorControl>> GetByToken(Guid token)
        {
            var sensorControl = await context.SensorControls
                .Include(e => e.Control)
                 .Include(e => e.Sensor)
                 .ThenInclude(e => e.SensorType)
                .Where(e => e.Control.Token == token ).ToListAsync();

            return sensorControl;
        }

        public override async Task<IEnumerable<SensorControl>> GetAll()
        {
            var sensorControl = await context.SensorControls
                .Include(e => e.Control)
                 .Include(e => e.Sensor).ToListAsync();

            return sensorControl;
        }

        public override async Task<SensorControl> GetById(int id)
        {
            var sensorControl = await context.SensorControls
                .Include(e => e.Control)
                 .Include(e => e.Sensor).FirstOrDefaultAsync(e=> e.Id == id);

            return sensorControl;
        }

        public SensorControl GetByControlIdAndSensorId(int controlId , int sensorId)
        {
            var sensorControl = context.SensorControls.FirstOrDefault(e => e.ControlId == controlId && e.SensorId == sensorId);

            return sensorControl;
        }
    }
}
