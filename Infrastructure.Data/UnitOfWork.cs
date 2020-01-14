using System;
using System.Collections.Generic;
using System.Text;
using Domain.Core.Model;
using Domain.Interfaces;
using Domain.Interfaces.Repositories;
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
        private IIconRepo _iconRepo;
        private IHistoryRepo _historyRepo;
        private INotificationRepository _notificationRepository;
		private IDashboardRepo _dashboardRepo;
		private IDashboardOptionsRepo _dashboardOptionsRepo;
		private IOptionsRepo _optionsRepo;
        private IReportElementRepo _reportElementRepo;

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
        public IIconRepo IconRepo
        {
            get
            {
                if (_iconRepo == null) _iconRepo = new IconRepo(context);
                return _iconRepo;
            }
        }

        public IReportElementRepo ReportElementRepo
        {
            get
            {
                if (_reportElementRepo == null) _reportElementRepo = new ReportElementRepo(context);
                return _reportElementRepo;
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

		public IDashboardRepo DashboardRepo
		{
			get
			{
				if (_dashboardRepo == null) _dashboardRepo = new DashboardRepo(context);
				return _dashboardRepo;
			}
		}

		public IDashboardOptionsRepo DashboardOptionsRepo
		{
			get
			{
				if (_dashboardOptionsRepo == null) _dashboardOptionsRepo = new DashboardOptionsRepo(context);
				return _dashboardOptionsRepo;
			}
		}

		public IOptionsRepo OptionsRepo
		{
			get
			{
				if (_optionsRepo == null) _optionsRepo = new OptionsRepo(context);
				return _optionsRepo;
			}
		}

		public int Save()
        {
            return context.SaveChanges();
        }
    }
}
