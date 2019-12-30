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

        private IGenericRepository<Sensor> _sensorRepo;
        private IGenericRepository<SensorType> _sensorTypeRepo;
        private IHistoryRepo _historyRepo;
        private INotificationRepository _notificationRepository;

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

        public IGenericRepository<Sensor> SensorRepo
        {
            get
            {
                if (_sensorRepo == null) _sensorRepo = new BaseRepository<Sensor>(context);
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
        public IGenericRepository<SensorType> SensorTypeRepo
        {
            get
            {
                if (_sensorTypeRepo == null) _sensorTypeRepo = new BaseRepository<SensorType>(context);
                return _sensorTypeRepo;
            }
        }
        public INotificationRepository NotificationRepository
        {
            get
            {
                if (_notificationRepository == null) _notificationRepository = new NotificationRepository(context);
                return _notificationRepository;
            }
        }
        public int Save()
        {
            return context.SaveChanges();
        }
    }
}
