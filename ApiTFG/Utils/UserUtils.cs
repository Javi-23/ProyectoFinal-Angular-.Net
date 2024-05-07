using ApiTFG.Repository.Contracts;

namespace ApiTFG.Utils
{
    public static class UserUtils
    {
        public static async Task<string> GetUserIdByUsername(IUserToFollowRepository userToFollowRepository, string username)
        {
            return await userToFollowRepository.GetUserIdByUsername(username);
        }
    }
}
