using Domain.Core.JoinModel;
using Infrastructure.Business.DTOs;
using Infrastructure.Business.DTOs.Dashboard;
using Infrastructure.Business.DTOs.DashboardOptions;
using Infrastructure.Business.Infrastructure;
using System.Threading.Tasks;

namespace Infrastructure.Business.Managers
{
    public interface IDashboardOptionsManager
    {
        Task<DashboardOptionsDto> GetByDashboardId(int id);
		Task<OperationDetails> Create(DashboardOptions dashboardOptions);
    }
}
