using Domain.Core.Model;
using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Infrastructure.Data.Repositories
{
    public class SensorRepo : BaseRepository<Sensor>, ISensorRepo
    {
        public SensorRepo(ApplicationsDbContext context) : base(context)
        {

        }

        public override IEnumerable<Sensor> GetAll()
        {
            var sensors = context.Sensors
                .Include(s => s.SensorType)
                    .ThenInclude(st => st.Icon)
                .Include(s => s.Icon);

            return sensors;
        }

        public override Sensor GetById(int id)
        {
            return context.Sensors.Find(id);
        }

    }
}
