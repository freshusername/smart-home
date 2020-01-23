using Domain.Core.Model;
using Microsoft.AspNetCore.Identity;

namespace Domain.Interfaces.Repositories
{
    public interface IUnitOfWork
    {
        IHistoryRepo HistoryRepo { get; }
        IMessageRepository MessageRepository { get; }
        ISensorRepo SensorRepo { get; }
        ISensorTypeRepo SensorTypeRepo { get; }
        IIconRepo IconRepo { get; }
        IDashboardRepo DashboardRepo { get; }
        IReportElementRepo ReportElementRepo { get; }
        INotificationRepo NotificationRepo { get; }
        UserManager<AppUser> UserManager { get; }
        RoleManager<IdentityRole> RoleManager { get; }
        SignInManager<AppUser> SignInManager { get; }
        int Save();
    }
}
