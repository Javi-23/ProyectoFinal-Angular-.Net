using ApiTFG.NewFolder;

namespace ApiTFG.Services.User
{
    public interface IUserService
    {
        Task<UserViewModel> GetUserByUserIdAsync(string userId);
        Task<UserViewModel> GetUserByUserNameAsync(string userName);
        Task<UserViewModel> UpdateUserDescriptionAsync(string token, string newDescription);
        Task<UserViewModel> GetMyProfile();
        Task<List<UserViewModel>> GetAllUsers();
        Task<List<UserViewModel>> GetUsersByUsernameStartingWithAsync(string prefix);
        Task<UserViewModel> UpdateUserImageAsync(string token, IFormFile imageFile);  
    }
}
