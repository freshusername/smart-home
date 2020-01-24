using Infrastructure.Business.DTOs.Icon;
using Infrastructure.Business.Infrastructure;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace Infrastructure.Business.Managers
{
	public interface IIconManager
    {
        Task<OperationDetails> Create(IconDto iconDto);
        Task<OperationDetails> Update(IconDto iconDto);
        Task<int> CreateAndGetIconId(IFormFile formFile);
        Task<IconDto> GetById(int id);
    }
}
