using ApiTFG.Dtos;

namespace ApiTFG.Services.UserAndPostService
{
    public interface IUserAndPostService
    {
        Task<UserAndPostDto> GetUserPosts(string username);
        Task<List<UserAndPostDto>> GetAllPosts();
        Task<List<UserAndPostDto>> GetFollowedPosts();

    }
}
