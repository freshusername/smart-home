using Domain.Core.Model;
using Domain.Interfaces;
using Domain.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Data.Repositories
{
    public class SensorTypeRepo : BaseRepository<SensorType>, ISensorTypeRepo
    {
        public SensorTypeRepo(ApplicationsDbContext context) : base(context)
        {

        }

        public override async Task<SensorType> GetById(int id)
        {
            var sensorTypes = await context.SensorTypes
                                    .Include(i => i.Icon)
                                    .FirstOrDefaultAsync(st => st.Id == id);

            return sensorTypes;
        }

        public override async Task<IEnumerable<SensorType>> GetAll()
        {
            var sensorTypes = await context.SensorTypes
                .Include(i => i.Icon).ToListAsync();

            return sensorTypes;
        }
    }
}

