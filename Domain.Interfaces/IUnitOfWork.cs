using System;
using System.Collections.Generic;
using System.Text;
using Domain.Core.Model;
using Microsoft.AspNetCore.Identity;

namespace Domain.Interfaces
{
    public interface IUnitOfWork
    {
        ISensorRepo SensorRepo { get; }
        ISensorTypeRepo SensorTypeRepo { get; }
        IHistoryRepo HistoryRepo { get; }
        UserManager<AppUser> UserManager { get; }
        RoleManager<IdentityRole> RoleManager { get; }
        SignInManager<AppUser> SignInManager { get; }
        int Save();
    }
}
