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

        public async Task<UserViewModel> GetMyProfile()
        {
            try
            {
                var username = GetUsername();
                if (username == null)
                {
                    throw new ArgumentException($"No se encontró ningún nombre de usuario.");
                }

                var user = await GetUserByUsernameAsync(username);
                return new UserViewModel
                {
                    UserName = user.UserName,
                    Description = user.Description,
                    Image = user.Image
                };
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<UserViewModel> GetUserByUsernameAsync(string username)
        {
            try
            {
                var user = await _dbContext.Users.SingleOrDefaultAsync(u => u.UserName == username);
                if (user == null)
                {
                    throw new ArgumentException($"No se encontró ningún usuario con el nombre deusuario '{username}'.");
                }

                return new UserViewModel
                {
                    UserName = user.UserName,
                    Description = user.Description,
                    Image = user.Image
                };
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<UserViewModel> UpdateUserDescriptionAsync(string token, string newDescription)
        {
            try
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
                    Description = user.Description,
                    Image = user.Image
                };
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<List<UserViewModel>> GetAllUsers()
        {
            try
            {
                var users = await _dbContext.Users.ToListAsync();
                return users.Select(user => new UserViewModel
                {
                    UserName = user.UserName,
                    Description = user.Description,
                    Image = user.Image
                }).ToList();
            }
            catch (Exception ex)
            {
                throw; 
            }
        }
        public async Task<List<UserViewModel>> GetUsersByUsernameStartingWithAsync(string prefix)
        {
            try
            {
                var users = await _dbContext.Users.Where(u => u.UserName.StartsWith(prefix)).ToListAsync();

                return users.Select(user => new UserViewModel
                {
                    UserName = user.UserName,
                    Description = user.Description,
                    Image = user.Image
                }).ToList();
            }
            catch (Exception ex)
            {
                throw;
            }
        }


        private string GetUsername()
        {
            var jwtToken = JwtUtils.ExtractJwtToken(_httpContextAccessor.HttpContext);
            return JwtUtils.ExtractUsernameFromToken(jwtToken);
        }

        public async Task<UserViewModel> UpdateUserImageAsync(string token, IFormFile imageFile)
        {
            try
            {
                var jwtToken = JwtUtils.ExtractJwtToken(_httpContextAccessor.HttpContext);
                var username = JwtUtils.ExtractUsernameFromToken(jwtToken);

                var user = await _dbContext.Users.SingleOrDefaultAsync(u => u.UserName == username);
                if (user == null)
                {
                    throw new ArgumentException($"No se encontró ningún usuario con el nombre de usuario '{username}'.");
                }

                if (imageFile != null && imageFile.Length > 0)
                {
                    using (var memoryStream = new MemoryStream())
                    {
                        await imageFile.CopyToAsync(memoryStream);
                        user.Image = memoryStream.ToArray();
                    }

                    await _dbContext.SaveChangesAsync();
                }

                return new UserViewModel
                {
                    UserName = user.UserName,
                    Description = user.Description,
                    Image = user.Image
                };
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}


