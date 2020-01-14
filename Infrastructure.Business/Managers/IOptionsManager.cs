using Domain.Core.Model;
using Infrastructure.Business.DTOs;
using Infrastructure.Business.DTOs.Dashboard;
using Infrastructure.Business.DTOs.Options;
using Infrastructure.Business.Infrastructure;
using System.Threading.Tasks;

namespace Infrastructure.Business.Managers
{
    public interface IOptionsManager
    {
        Task<OptionsDto> GetById(int id);
		Task<OperationDetails> Create(Options options);
    }
}
