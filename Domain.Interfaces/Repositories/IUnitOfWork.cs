using Domain.Core.Model;
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
        IReportElementRepo ReportElementRepo { get; }
        IControlRepo ControlRepo { get; }
        ISensorControlRepo SensorControlRepo { get; }
        UserManager<AppUser> UserManager { get; }
        RoleManager<IdentityRole> RoleManager { get; }
        SignInManager<AppUser> SignInManager { get; }
        int Save();
    }
}
