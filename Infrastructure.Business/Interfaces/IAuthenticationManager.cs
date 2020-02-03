using Infrastructure.Business.DTOs;
using Infrastructure.Business.Infrastructure;
using System.Threading.Tasks;

namespace Infrastructure.Business.Interfaces
{
    public interface IAuthenticationManager
    {
        Task<OperationDetails> Register(UserDTO userDTO);
        Task<OperationDetails> Login(UserDTO userDTO);
        Task<ConfirmDto> GetPasswordConfirmationToken(string userName);
        Task<ConfirmDto> GetEmailConfirmationToken(string userName);
        Task<OperationDetails> GoogleAuthentication();
		Task Logout();
	}
}
