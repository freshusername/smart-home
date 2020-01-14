using Domain.Core.Model;
using Infrastructure.Business.DTOs;
using Infrastructure.Business.DTOs.Dashboard;
using Infrastructure.Business.Infrastructure;
using System.Threading.Tasks;

namespace Infrastructure.Business.Managers
{
    public interface IDashboardManager
    {
        Task<DashboardDto> GetById(int id);
		Task<OperationDetails> Create(DashboardDto dashboardDto);
    }
}
