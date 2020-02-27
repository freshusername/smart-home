using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Domain.Core.Model;
using Domain.Interfaces.Repositories;
using Infrastructure.Business.DTOs.Dashboard;
using Infrastructure.Business.Infrastructure;
using System.Linq;
using System;
using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Business.Managers
{
    public class DashboardManager : BaseManager, IDashboardManager
    {
        private UserManager<AppUser> _userManager { get; set; }

        public DashboardManager(IUnitOfWork unitOfWork, IMapper mapper, UserManager<AppUser> userManager) : base(unitOfWork, mapper)
        {
            _userManager = userManager;
        }

        public async Task<IEnumerable<DashboardDto>> GetAll()
        {
            var dashboards = await unitOfWork.DashboardRepo.GetAll();
            return mapper.Map<IEnumerable<Dashboard>, IEnumerable<DashboardDto>>(dashboards);
        }

        public async Task<IEnumerable<DashboardDto>> GetByUserId(string userId)
        {
            var dashboards = await unitOfWork.DashboardRepo.GetAll();
            dashboards = dashboards.Where(d => d.AppUserId == userId);
            return mapper.Map<IEnumerable<Dashboard>, IEnumerable<DashboardDto>>(dashboards);
        }

        public async Task<IEnumerable<DashboardDto>> GetAllPublic(string userId)
        {
            var dashboards = await unitOfWork.DashboardRepo.GetAllPublic(userId);
            return mapper.Map<IEnumerable<Dashboard>, IEnumerable<DashboardDto>>(dashboards);
        }

        public async Task<DashboardDto> GetById(int id)
        {
            var dashboard = await unitOfWork.DashboardRepo.GetById(id);
            var result = mapper.Map<Dashboard, DashboardDto>(dashboard);

            return result;
        }

        public async Task<DashboardDto> Create(DashboardDto dashboardDto)
        {
            Dashboard dashboard = mapper.Map<DashboardDto, Dashboard>(dashboardDto);
            try
            {
                await unitOfWork.DashboardRepo.Insert(dashboard);
                unitOfWork.Save();
            }
            catch (Exception)
            {
                return null;
            }
            return mapper.Map<Dashboard, DashboardDto>(dashboard);
        }

        public async Task<DashboardDto> Update(DashboardDto dashboardDto)
        {
            Dashboard dashboard = mapper.Map<DashboardDto, Dashboard>(dashboardDto);
            try
            {
                await unitOfWork.DashboardRepo.Update(dashboard);
                unitOfWork.Save();
            }
            catch (Exception)
            {
                return null;
            }
            return mapper.Map<Dashboard, DashboardDto>(dashboard);
        }

        public async Task<OperationDetails> Delete(int id)
        {
            try
            {
                await unitOfWork.DashboardRepo.DeleteById(id);
                unitOfWork.Save();
            }
            catch (Exception ex)
            {
                return new OperationDetails(false, ex.Message, "Error");
            }
            return new OperationDetails(true, "Dashboard has been deleted", "Name");
        }
    }
}
