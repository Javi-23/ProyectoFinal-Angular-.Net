using ApiTFG.Models;

namespace ApiTFG.Repository.Contracts
{
    public interface IUserToFollowRepository : IGenericRepository<UserToFollows>
    {
        Task<UserToFollows> GetByFollowerAndFollowed(string followerId, string followedId);
        public Task<string> GetUserIdByUsername(string username);
        public Task<List<string>> GetFollowedId(string username);
        public Task<List<string>> GetFollowerId(string username);
    }
}
