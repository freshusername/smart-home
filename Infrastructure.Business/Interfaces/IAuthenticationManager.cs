using Infrastructure.Business.DTOs;
using Infrastructure.Business.Infrastructure;
using System.Threading.Tasks;

namespace Infrastructure.Business.Managers
{
    public interface IAuthenticationManager
    {
        Task<OperationDetails> Register(UserDTO userDTO);
        Task<OperationDetails> Login(UserDTO userDTO);
        Task<ConfirmDto> GetPasswordConfirmationToken(string userName);
        Task<ConfirmDto> GetEmailConfirmationToken(string userName);
        Task Logout();
        Task<OperationDetails> GoogleAuthentication();
    }
}
