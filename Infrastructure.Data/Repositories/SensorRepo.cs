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
        private readonly ApplicationsDbContext _context;
        public SensorRepo(ApplicationsDbContext context) : base(context)
        {
            _context = context;
        }

        public override IEnumerable<Sensor> GetAll()
        {
            var sensors = _context.Sensors
                .Include(s => s.SensorType)
                    .ThenInclude(i => i.Icon);

            return sensors;
        }

        public override Sensor GetById(int id)
        {
            return _context.Sensors.Find(id);
        }

    }
}
