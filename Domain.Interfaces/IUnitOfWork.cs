using System;
using System.Collections.Generic;
using System.Text;
using Domain.Core.Model;
using Microsoft.AspNetCore.Identity;

namespace Domain.Interfaces
{
    public interface IUnitOfWork
    {
        IHistoryRepo HistoryRepo { get; }
        INotificationRepository NotificationRepository { get; }
        ISensorRepo SensorRepo { get; }
        ISensorTypeRepo SensorTypeRepo { get; }
        UserManager<AppUser> UserManager { get; }
        RoleManager<IdentityRole> RoleManager { get; }
        SignInManager<AppUser> SignInManager { get; }
        int Save();
    }
}
