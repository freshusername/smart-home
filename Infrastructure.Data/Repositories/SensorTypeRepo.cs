using Domain.Core.Model;
using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Data.Repositories
{
    public class SensorTypeRepo : BaseRepository<SensorType>, ISensorTypeRepo
    {
        public SensorTypeRepo(ApplicationsDbContext context) : base(context)
        {

        }

        public override IEnumerable<SensorType> GetAll()
        {
            var sensorTypes = context.SensorTypes
                .Include(i => i.Icon);

            return sensorTypes;
        }
    }
}

