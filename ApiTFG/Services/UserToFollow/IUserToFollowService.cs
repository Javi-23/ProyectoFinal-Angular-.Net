using ApiTFG.NewFolder;

namespace ApiTFG.Services.UserToFollow
{
    public interface IUserToFollowService
    {
        Task<bool> FollowUser(string followedUsername);
        Task<bool> UnfollowUser(string followedUsername);
        Task<List<UserViewModel>> GetFollowedUsers(string username);
        Task<List<UserViewModel>> GetFollowerUsers(string username);

    }
}
