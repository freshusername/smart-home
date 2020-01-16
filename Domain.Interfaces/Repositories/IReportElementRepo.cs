using Domain.Core.Model;
using System.Threading.Tasks;

namespace Domain.Interfaces.Repositories
{
    public interface IReportElementRepo : IGenericRepository<ReportElement>
    {
        Task<bool> ReportElementExist(ReportElement reportElement);
    }
}
