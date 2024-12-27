using DotaProject.Models;
using DotaProject.Services.Interfaces;
using DotaProject.Data.Repositories.Interfaces;
using DotaProject.Security;
using Serilog;

namespace DotaProject.Services;

public class UserService(IUserRepository userRepository) : IUserService
{
    public async Task<IEnumerable<User>> GetAllUsersAsync()
    {
        return await userRepository.GetAllUsersAsync();
    }

    public async Task<User?> GetUserByIdAsync(int id)
    {
        return await userRepository.GetUserByIdAsync(id);
    }

    public async Task<User?> GetUserByUsernameAsync(string username)
    {
        return await userRepository.GetUserByUsernameAsync(username);
    }

    public async Task<User> AddUserAsync(User user)
    {
        
        user.Password = PasswordEncryptor.HashPassword(user.Password);

        
        user.Claims = new List<UserClaim>
        {
            new UserClaim
            {
                ClaimType = "role",
                ClaimValue = "verifiedUser"
            }
        };

        return await userRepository.AddUserAsync(user);
    }

    public async Task<User?> UpdateUserAsync(int id, User updatedUser)
    {
        return await userRepository.UpdateUserAsync(id, updatedUser);
    }

    public async Task<bool> DeleteUserAsync(int id)
    {
        return await userRepository.DeleteUserAsync(id);
    }
    
    //TODO Global error handling
    public async Task<bool> SetVerifiedUserAsync(int userId)
    {
        try
        {
            await userRepository.SetVerifiedUserAsync(userId);
            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Failed to set user {userId} as 'verifiedUser': {ex.Message}");
            return false;
        }
    }

    public async Task<bool> SetAdminAsync(int userId)
    {
        try
        {
            await userRepository.SetAdminAsync(userId);
            Log.Information($"Attempting to set user with ID {userId} as admin.");
            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Failed to set user {userId} as 'admin': {ex.Message}");
            return false;
        }
    }
}