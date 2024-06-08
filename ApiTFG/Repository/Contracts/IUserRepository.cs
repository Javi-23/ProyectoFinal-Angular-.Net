using ApiTFG.Dtos;
using ApiTFG.Models;


namespace ApiTFG.Repository.Contracts
{
    public interface IUserRepository : IGenericRepository<AppUser>
    {
        public Task<string> GetUserIdByUsername(string username);

        public Task<List<AppUser>> GetAllUsersWithPosts();

    }
}
