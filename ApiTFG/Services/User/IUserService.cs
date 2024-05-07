using ApiTFG.NewFolder;

namespace ApiTFG.Services.User
{
    public interface IUserService
    {
        Task<UserViewModel> GetUserByUsernameAsync(string username);
        Task<UserViewModel> UpdateUserDescriptionAsync(string token, string newDescription);
    }
}
