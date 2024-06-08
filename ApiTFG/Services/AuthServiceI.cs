using ApiTFG.Dtos;
using ApiTFG.Models;
using Microsoft.AspNetCore.Identity;

namespace ApiTFG.Services
{
    public interface IAuthService
    {
        Task<string> GenerateTokenString(LoginDTO user);
        Task<bool> Login(LoginDTO user);
        Task<IdentityResult> RegisterUser(LoginUser user);
        Task<bool> IsTokenValid(string token);
    }
}
