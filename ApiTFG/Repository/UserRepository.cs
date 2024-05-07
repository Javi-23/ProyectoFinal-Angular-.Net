using ApiTFG.Data;
using ApiTFG.Dtos;
using ApiTFG.Models;
using ApiTFG.Repository.Contracts;
using Microsoft.EntityFrameworkCore;

namespace ApiTFG.Repository
{
    public class UserRepository : GenericRepository<AppUser>, IUserRepository
    {
        private readonly AppDbContext _appDbContext;

        public UserRepository(AppDbContext context) : base(context)
        {
            _appDbContext = context;
        }

        public async Task<List<AppUser>> GetAllUsersWithPosts()
        {
            return await _appDbContext.Users
                 .Include(u => u.UserPosts)
                 .ToListAsync();
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
