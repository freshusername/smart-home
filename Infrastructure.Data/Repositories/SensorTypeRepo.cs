using Domain.Core.Model;
using Domain.Interfaces;
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


    }
}

