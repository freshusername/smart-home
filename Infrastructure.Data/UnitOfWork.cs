using System;
using System.Collections.Generic;
using System.Text;
using Domain.Core.Model;
using Domain.Interfaces;
using Infrastructure.Data.Repositories;

namespace Infrastructure.Data
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationsDbContext context;

        private IGenericRepository<Sensor> sensorRepo;

        public UnitOfWork(ApplicationsDbContext dbContext)
        {
            context = dbContext;
        }

        public IGenericRepository<Sensor> SensorRepo
        {
            get
            {
                if (sensorRepo == null) sensorRepo = new BaseRepository<Sensor>(context);
                return sensorRepo;
            }
        }

        public int Save()
        {
            return context.SaveChanges();
        }
    }
}
