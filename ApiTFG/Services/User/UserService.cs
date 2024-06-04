using ApiTFG.Data;
using ApiTFG.Exceptions;
using ApiTFG.Models;
using ApiTFG.NewFolder;
using ApiTFG.Utils;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

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
                var userId = GetUserId();
                if (userId == null)
                {
                    throw new UserException($"Failed to retrieve the data");
                }

                var user = await GetUserByUserIdAsync(userId);
                return new UserViewModel
                {
                    UserName = user.UserName,
                    Description = user.Description,
                    Image = user.Image
                };
            }
            catch (Exception ex)
            {
                throw new UserException("Error retrieving user profile", ex);
            }
        }

        public async Task<UserViewModel> UpdateUserDescriptionAsync(string token, string newDescription)
        {
            try
            {
                var jwtToken = JwtUtils.ExtractJwtToken(_httpContextAccessor.HttpContext);
                var userId = JwtUtils.ExtractUserIdFromToken(jwtToken);

                var user = await _dbContext.Users.SingleOrDefaultAsync(u => u.Id == userId);
                if (user == null)
                {
                    throw new UserException($"No user found with user ID '{userId}'.");
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
                throw new UserException("Error updating user description", ex);
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
                throw new UserException("Error retrieving all users", ex);
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
                throw new UserException("Error retrieving users by username starting with the specified prefix", ex);
            }
        }

        public async Task<UserViewModel> GetUserByUserIdAsync(string userId)
        {
            try
            {
                var user = await _dbContext.Users.SingleOrDefaultAsync(u => u.Id == userId);
                if (user == null)
                {
                    throw new UserException($"No user found with ID '{userId}'.");

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
                throw new UserException($"No user found with ID '{userId}'.");
            }
        }

        public async Task<UserViewModel> GetUserByUserNameAsync(string userName)
        {
            try
            {
                var user = await _dbContext.Users.SingleOrDefaultAsync(u => u.UserName == userName);
                if (user == null)
                {
                    throw new UserException($"No user found with username '{userName}'.");
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
                throw new UserException("Error retrieving user by username", ex);
            }
        }

        private string GetUserId()
        {
            var jwtToken = JwtUtils.ExtractJwtToken(_httpContextAccessor.HttpContext);
            return JwtUtils.ExtractUserIdFromToken(jwtToken);
        }

        public async Task<UserViewModel> UpdateUserImageAsync(string token, IFormFile imageFile)
        {
            try
            {
                var jwtToken = JwtUtils.ExtractJwtToken(_httpContextAccessor.HttpContext);
                var userId = JwtUtils.ExtractUserIdFromToken(jwtToken);

                var user = await _dbContext.Users.SingleOrDefaultAsync(u => u.Id == userId);
                if (user == null)
                {
                    throw new UserException($"No user found with ID '{userId}'.");
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
                throw new UserException("Error updating user image", ex);
            }
        }
    }
}