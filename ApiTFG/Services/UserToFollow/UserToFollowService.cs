using ApiTFG.Data;
using ApiTFG.Exceptions;
using ApiTFG.Models;
using ApiTFG.NewFolder;
using ApiTFG.Repository.Contracts;
using ApiTFG.Utils;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ApiTFG.Services.UserToFollow
{
    public class UserToFollowService : IUserToFollowService
    {
        private readonly IUserToFollowRepository _userToFollowRepository;
        private readonly IUserRepository _userRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly AppDbContext _dbContext;

        public UserToFollowService(IUserToFollowRepository userToFollowRepository, IUserRepository userRepository, IHttpContextAccessor httpContextAccessor, AppDbContext appDbContext)
        {
            _userToFollowRepository = userToFollowRepository;
            _userRepository = userRepository;
            _httpContextAccessor = httpContextAccessor;
            _dbContext = appDbContext;
        }

        public async Task<bool> FollowUser(string followedUsername)
        {
            try
            {
                var followerId = await GetCurrentUserId();
                var followedId = await GetUserIdByUsername(followedUsername);

                if (followerId == followedId)
                    return false;

                var userToFollow = await _userToFollowRepository.GetByFollowerAndFollowed(followerId, followedId);
                if (userToFollow != null)
                    return false;

                await _userToFollowRepository.Create(new UserToFollows
                {
                    FollowerId = followerId,
                    FollowedId = followedId
                });

                return true;
            }
            catch (UserException ex)
            {
                throw new UserException($"Error al seguir al usuario '{followedUsername}'", ex);
            }
        }

        public async Task<bool> UnfollowUser(string followedUsername)
        {
            try
            {
                var followerId = await GetCurrentUserId();
                var followedId = await GetUserIdByUsername(followedUsername);

                if (followerId == followedId)
                    return false;

                var userToUnfollow = await _userToFollowRepository.GetByFollowerAndFollowed(followerId, followedId);
                if (userToUnfollow == null)
                    return false;

                await _userToFollowRepository.Delete(userToUnfollow);
                return true;
            }
            catch (UserException ex)
            {
                throw new UserException($"Error al dejar de seguir al usuario '{followedUsername}'", ex);
            }
        }

        public async Task<List<UserViewModel>> GetFollowedUsers(string username)
        {
            try
            {
                var followedUserIds = await _userToFollowRepository.GetFollowedId(username);
                return await GetUserViewModels(followedUserIds);
            }
            catch (ArgumentException ex)
            {
                throw new UserException($"Error al obtener los usuarios seguidos por '{username}'", ex);
            }
        }

        public async Task<List<UserViewModel>> GetFollowerUsers(string username)
        {
            try
            {
                var followerUserIds = await _userToFollowRepository.GetFollowerId(username);
                return await GetUserViewModels(followerUserIds);
            }
            catch (UserException ex)
            {
                throw new UserException($"Error al obtener los usuarios que siguen a '{username}'", ex);
            }
        }

        private async Task<string> GetUserIdByUsername(string username)
        {
            var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.UserName == username);
            if (user == null)
                throw new UserException($"No se encontró ningún usuario con el nombre '{username}'.");

            return user.Id;
        }

        private async Task<string> GetCurrentUserId()
        {
            var jwtToken = JwtUtils.ExtractJwtToken(_httpContextAccessor.HttpContext);
            return JwtUtils.ExtractUserIdFromToken(jwtToken);
        }

        private async Task<List<UserViewModel>> GetUserViewModels(List<string> userIds)
        {
            var users = new List<UserViewModel>();
            foreach (var userId in userIds)
            {
                var user = await _userRepository.Get(u => u.Id == userId);
                if (user != null)
                    users.Add(new UserViewModel { UserName = user.UserName, Description = user.Description });
            }
            return users;
        }
    }
}