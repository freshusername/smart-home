﻿using Domain.Core.Model;
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
                .Where(e => e.Control.Token == token ).ToListAsync();

            return sensorControl;
        }
    }
}