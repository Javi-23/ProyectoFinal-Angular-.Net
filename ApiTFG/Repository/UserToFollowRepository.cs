using ApiTFG.Data;
using ApiTFG.Models;
using ApiTFG.Repository.Contracts;
using Microsoft.EntityFrameworkCore;

namespace ApiTFG.Repository
{
    public class UserToFollowRepository : GenericRepository<UserToFollows>, IUserToFollowRepository
    {

        private readonly AppDbContext _appDbContext;

        public UserToFollowRepository(AppDbContext context) : base(context)
        {
            _appDbContext = context;
        }

        public async Task<UserToFollows> GetByFollowerAndFollowed(string followerId, string followedId)
        {
            var query = await Query(uf => uf.FollowerId == followerId && uf.FollowedId == followedId);
            return await query.FirstOrDefaultAsync();
        }

        public async Task<List<string>> GetFollowedId(string username)
        {
            var followedUserIds = await _appDbContext.UserToFollows
                .Where(uf => uf.Follower.UserName == username) 
                .Select(uf => uf.FollowedId)
                .ToListAsync();

            return followedUserIds;
        }

        public async Task<List<string>> GetFollowerId(string username)
        {
            var followedUserIds = await _appDbContext.UserToFollows
                .Where(uf => uf.Followed.UserName == username)
                .Select(uf => uf.FollowerId)
                .ToListAsync();

            return followedUserIds;
        }

        public async Task<string> GetUserIdByUsername(string username)
        {
            try
            {
                var user = await _appDbContext.Users.FirstOrDefaultAsync(u => u.UserName == username);
                return user?.Id;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
