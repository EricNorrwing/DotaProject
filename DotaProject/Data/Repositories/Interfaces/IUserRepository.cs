using DotaProject.Models;

namespace DotaProject.Data.Repositories.Interfaces
{
    public interface IUserRepository
    {
        Task<IEnumerable<User>> GetAllUsersAsync();
        Task<User?> GetUserByIdAsync(int id);
        Task<User?> GetUserByUsernameAsync(string username);
        Task<User> AddUserAsync(User user);
        Task<User?> UpdateUserAsync(int id, User updatedUser);
        Task<bool> DeleteUserAsync(int id);

        
        Task SetVerifiedUserAsync(int userId);
        Task SetAdminAsync(int userId);
    }
}