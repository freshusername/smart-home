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

        private IGenericRepository<Sensor> sensorRepo;
        private IGenericRepository<History> historyRepo;

        public UnitOfWork(ApplicationsDbContext dbContext , UserManager<AppUser> userManager , RoleManager<IdentityRole> roleManager , SignInManager<AppUser> signInManager)
        {
            context = dbContext;
            UserManager = userManager;
            RoleManager = roleManager;
            SignInManager = signInManager;
        }

        public IGenericRepository<Sensor> SensorRepo
        {
            get
            {
                if (sensorRepo == null) sensorRepo = new BaseRepository<Sensor>(context);
                return sensorRepo;
            }
        }

        public IGenericRepository<History> HistoryRepo
        {
            get
            {
                if (historyRepo == null) historyRepo = new BaseRepository<History>(context);
                return historyRepo;
            }
        }


        public int Save()
        {
            return context.SaveChanges();
        }
    }
}
