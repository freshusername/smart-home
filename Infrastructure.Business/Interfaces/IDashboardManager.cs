using Infrastructure.Business.DTOs.Dashboard;
using System.Threading.Tasks;

namespace Infrastructure.Business.Managers
{
    public interface IDashboardManager
    {
        Task<DashboardDto> GetById(int id);
    }
}
