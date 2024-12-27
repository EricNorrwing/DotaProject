using DotaProject.Models;

namespace DotaProject.Services.Interfaces
{
    public interface IUserService
    {
        Task<IEnumerable<User>> GetAllUsersAsync();
        Task<User?> GetUserByIdAsync(int id);
        Task<User?> GetUserByUsernameAsync(string username);
        Task<User> AddUserAsync(User user);
        Task<User?> UpdateUserAsync(int id, User updatedUser);
        Task<bool> DeleteUserAsync(int id);

        
        Task<bool> SetVerifiedUserAsync(int userId);
        Task<bool> SetAdminAsync(int userId);
    }
}