using ApiTFG.Data;
using ApiTFG.Models;
using ApiTFG.NewFolder;
using ApiTFG.Utils;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace ApiTFG.Services.User
{
    public class UserService : IUserService
    {
        private readonly AppDbContext _dbContext;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserService(AppDbContext dbContext, IHttpContextAccessor httpContextAccessor)
        {
            _dbContext = dbContext;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<UserViewModel> GetUserByUsernameAsync(string username)
        {
            var user = await _dbContext.Users.SingleOrDefaultAsync(u => u.UserName == username);
            if (user == null)
            {
                throw new ArgumentException($"No se encontró ningún usuario con el nombre de usuario '{username}'.");
            }

            return new UserViewModel
            {
                UserName = user.UserName,
                Description = user.Description
            };
        }
        public async Task<UserViewModel> UpdateUserDescriptionAsync(string token, string newDescription)
        {
            var jwtToken = JwtUtils.ExtractJwtToken(_httpContextAccessor.HttpContext);
            var username = JwtUtils.ExtractUsernameFromToken(jwtToken);

            var user = await _dbContext.Users.SingleOrDefaultAsync(u => u.UserName == username);
            if (user == null)
            {
                throw new ArgumentException($"No se encontró ningún usuario con el nombre de usuario '{username}'.");
            }

            user.Description = newDescription;
            await _dbContext.SaveChangesAsync();

            return new UserViewModel
            {
                UserName = user.UserName,
                Description = user.Description
            };
        }
    }
}