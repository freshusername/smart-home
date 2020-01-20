using Domain.Core.Model;
using Domain.Core.Model.Enums;
using Domain.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Data.Repositories
{
    public class SensorControlRepo : BaseRepository<SensorControl>, ISensorControlRepo
    {
        public SensorControlRepo(ApplicationsDbContext context) : base(context)
        {

        }

        public async Task<SensorControl> GetByTokenAndRople(Guid token , ActionRole role)
        {
            var sensorControl = await context.SensorControls
                .Include(e => e.Control)
                 .Include(e => e.Sensor)
                .FirstOrDefaultAsync(e => e.Control.Token == token && e.Role == role);

            return sensorControl;
        }
    }
}
