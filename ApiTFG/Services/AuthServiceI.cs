using ApiTFG.Dtos;
using ApiTFG.Models;

namespace ApiTFG.Services
{
    public interface IAuthService
    {
        Task<string> GenerateTokenString(LoginDTO user);
        Task<bool> Login(LoginDTO user);
        Task<bool> RegisterUser(LoginUser user);
        Task<bool> IsTokenValid(string token);
    }
}
