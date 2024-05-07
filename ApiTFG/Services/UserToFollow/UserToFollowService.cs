using ApiTFG.Repository.Contracts;
using ApiTFG.Utils;
using ApiTFG.Models;
using System.Security.Claims;
using ApiTFG.NewFolder;
using ApiTFG.Repository;

namespace ApiTFG.Services.UserToFollow
{
    public class UserToFollowService : IUserToFollowService
    {
        private readonly IUserToFollowRepository _userToFollowRepository;
        private readonly IUserRepository _userRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserToFollowService(IUserToFollowRepository userToFollowRepository, IUserRepository userRepository, IHttpContextAccessor httpContextAccessor)
        {
            _userToFollowRepository = userToFollowRepository;
            _userRepository = userRepository;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<bool> FollowUser(string followedUsername)
        {
            try
            {
                var followerUsername = GetUsername();
                var followerId = await GetUserIdByUsername(followerUsername);
                var followedId = await GetUserIdByUsername(followedUsername);

                if (followerId == followedId)
                {
                    return false;
                }

                var userToFollow = await _userToFollowRepository.GetByFollowerAndFollowed(followerId, followedId);
                if (userToFollow != null)
                {
                    return false;
                }

                var relationship = new Models.UserToFollows
                {
                    FollowerId = followerId,
                    FollowedId = followedId
                };
                await _userToFollowRepository.Create(relationship);

                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<bool> UnfollowUser(string followedUsername)
        {
            try
            {
                var followerUsername = GetUsername();
                var followerId = await GetUserIdByUsername(followerUsername);
                var followedId = await GetUserIdByUsername(followedUsername);

                if (followerId == followedId)
                {
                    return false;
                }

                var userToUnfollow = await _userToFollowRepository.GetByFollowerAndFollowed(followerId, followedId);
                if (userToUnfollow == null)
                {
                    return false;
                }

                await _userToFollowRepository.Delete(userToUnfollow);

                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<List<UserViewModel>> GetFollowedUsers(string username)
        {
            try
            {
                var followedUserIds = await _userToFollowRepository.GetFollowedId(username);
                return await GetUserViewModels(followedUserIds);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<List<UserViewModel>> GetFollowerUsers(string username)
        {
            try
            {
                var followedUserIds = await _userToFollowRepository.GetFollowerId(username);
                return await GetUserViewModels(followedUserIds);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private async Task<string> GetUserIdByUsername(string username)
        {
            return await _userToFollowRepository.GetUserIdByUsername(username);
        }

        private async Task<List<UserViewModel>> GetUserViewModels(List<string> userIds)
        {
            var users = new List<UserViewModel>();

            foreach (var userId in userIds)
            {
                var user = await _userRepository.Get(u => u.Id == userId);
                if (user != null)
                {
                    var userViewModel = new UserViewModel
                    {
                        UserName = user.UserName,
                        Description = user.Description
                    };
                    users.Add(userViewModel);
                }
            }

            return users;
        }

        private string GetUsername()
        {
            var jwtToken = JwtUtils.ExtractJwtToken(_httpContextAccessor.HttpContext);
            return JwtUtils.ExtractUsernameFromToken(jwtToken);
        }
    }
}