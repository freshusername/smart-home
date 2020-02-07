using Domain.Core.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Domain.Interfaces.Repositories
{
    public interface IDashboardRepo : IGenericRepository<Dashboard>
    {
        Task<IEnumerable<Dashboard>> GetAllPublic(string userId);
        Task<Dashboard> GetLastDashboard();
    }
}
