using Domain.Core.Model;
using Infrastructure.Business.DTOs;
using Infrastructure.Business.DTOs.Dashboard;
using Infrastructure.Business.Infrastructure;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Infrastructure.Business.Managers
{
    public interface IDashboardManager
    {
        Task<DashboardDto> GetById(int id);
        Task<IEnumerable<DashboardDto>> GetAll();
		Task<OperationDetails> Create(DashboardDto dashboardDto);
        Task Update(int id,string name);
        Task<OperationDetails> DeleteById(int id);
    }
}
