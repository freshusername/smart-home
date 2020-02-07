﻿using Domain.Core.Model;
using Infrastructure.Business.DTOs;
using Infrastructure.Business.DTOs.Dashboard;
using Infrastructure.Business.Infrastructure;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Infrastructure.Business.Interfaces
{
    public interface IDashboardManager
    {
        Task<DashboardDto> GetById(int id);
        Task<IEnumerable<DashboardDto>> GetAll();
        Task<IEnumerable<DashboardDto>> GetByUserId(string userId);
        Task<IEnumerable<DashboardDto>> GetAllPublic(string userId);

        OperationDetails Update(DashboardDto dashboardDto);

        Task<OperationDetails> Create(DashboardDto dashboardDto);
        Task<OperationDetails> DeleteById(int id);
        Task<DashboardDto> GetLastDashboard();
    }
}
