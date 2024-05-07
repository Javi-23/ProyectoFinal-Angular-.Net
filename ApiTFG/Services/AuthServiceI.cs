using ApiTFG.Dtos;
using ApiTFG.Models;

namespace ApiTFG.Services
{
    public interface IAuthService
    {
        string GenerateTokenString(LoginDTO user);
        Task<bool> Login(LoginDTO user);
        Task<bool> RegisterUser(LoginUser user);
    }
}
