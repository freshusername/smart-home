using System;
using System.Collections.Generic;
using System.Text;
using Domain.Core.Model;
using Domain.Interfaces;
using Infrastructure.Data.Repositories;
using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Data
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationsDbContext context;
        public UserManager<AppUser> UserManager { get; private set; }
        public RoleManager<IdentityRole> RoleManager { get; private set; }
        public SignInManager<AppUser> SignInManager { get; private set; }

        private ISensorRepo _sensorRepo;
        private ISensorTypeRepo _sensorTypeRepo;
        private IGenericRepository<History> _historyRepo;

        public UnitOfWork(
            ApplicationsDbContext dbContext,
            UserManager<AppUser> userManager,
            RoleManager<IdentityRole> roleManager,
            SignInManager<AppUser> signInManager)
        {
            context = dbContext;
            UserManager = userManager;
            RoleManager = roleManager;
            SignInManager = signInManager;
        }

        public ISensorRepo SensorRepo
        {
            get
            {
                if (_sensorRepo == null) _sensorRepo = new SensorRepo(context);
                return _sensorRepo;
            }
        }

        public IHistoryRepo HistoryRepo
        {
            get
            {
                if (_historyRepo == null) _historyRepo = new HistoryRepo(context);
                return _historyRepo;
            }
        }
        public ISensorTypeRepo SensorTypeRepo
        {
            get
            {
                if (_sensorTypeRepo == null) _sensorTypeRepo = new SensorTypeRepo(context);
                return _sensorTypeRepo;
            }
        }

        public int Save()
        {
            return context.SaveChanges();
        }
    }
}
