﻿using Domain.Core.Model;
using Microsoft.AspNetCore.Identity;

namespace Domain.Interfaces.Repositories
{
    public interface IUnitOfWork
    {
        IHistoryRepo HistoryRepo { get; }
        INotificationRepository NotificationRepository { get; }
        ISensorRepo SensorRepo { get; }
        ISensorTypeRepo SensorTypeRepo { get; }
        IIconRepo IconRepo { get; }
        IDashboardRepo DashboardRepo { get; }
        IDashboardOptionsRepo DashboardOptionsRepo { get; }
        IOptionsRepo OptionsRepo { get; }
        IReportElementRepo ReportElementRepo { get; }
        UserManager<AppUser> UserManager { get; }
        RoleManager<IdentityRole> RoleManager { get; }
        SignInManager<AppUser> SignInManager { get; }
        int Save();
    }
}