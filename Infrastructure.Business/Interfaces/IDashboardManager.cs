using Infrastructure.Business.DTOs.Dashboard;
using System.Threading.Tasks;

namespace Infrastructure.Business.Interfaces
{
    public interface IDashboardManager
    {
        Task<DashboardDto> GetById(int id);
    }
}
