

using DotaProject.Data.Repositories.DbContexts;
using DotaProject.Data.Repositories.Interfaces;
using DotaProject.Models;
using Microsoft.EntityFrameworkCore;

namespace DotaProject.Data.Repositories;

public class UserRepository(AuthDbContext context) : IUserRepository
{
    public async Task<IEnumerable<User>> GetAllUsersAsync()
    {
        return await context.Users.ToListAsync();
    }

    public async Task<User?> GetUserByIdAsync(int id)
    {
        return await context.Users
            .Include(u => u.Claims) 
            .FirstOrDefaultAsync(u => u.Id == id);
    }

    public async Task<User?> GetUserByUsernameAsync(string username)
    {
        return await context.Users
            .Include(u => u.Claims) 
            .FirstOrDefaultAsync(u => u.Username == username);
    }

    public async Task<User> AddUserAsync(User user)
    {
        context.Users.Add(user);
        await context.SaveChangesAsync();
        return user;
    }

    public async Task<User?> UpdateUserAsync(int id, User updatedUser)
    {
        var user = await context.Users.FindAsync(id);
        if (user == null) return null;

        user.Username = updatedUser.Username;
        user.Password = updatedUser.Password;

        await context.SaveChangesAsync();
        return user;
    }

    public async Task<bool> DeleteUserAsync(int id)
    {
        var user = await context.Users.FindAsync(id);
        if (user == null) return false;

        context.Users.Remove(user);
        await context.SaveChangesAsync();
        return true;
    }
    
    public async Task SetVerifiedUserAsync(int userId)
    {
        var claim = await context.UserClaims
            .FirstOrDefaultAsync(c => c.UserId == userId && c.ClaimType == "role");

        if (claim != null)
        {
            claim.ClaimValue = "verifiedUser";
            context.UserClaims.Update(claim);
        }
        else
        {
            context.UserClaims.Add(new UserClaim
            {
                UserId = userId,
                ClaimType = "role",
                ClaimValue = "verifiedUser"
            });
        }

        await context.SaveChangesAsync();
    }

    public async Task SetAdminAsync(int userId)
    {
        var claim = await context.UserClaims
            .FirstOrDefaultAsync(c => c.UserId == userId && c.ClaimType == "role");

        if (claim != null)
        {
            // Update the claim to "admin"
            claim.ClaimValue = "admin";
            context.UserClaims.Update(claim);
        }
        else
        {
            // Add the "admin" claim
            context.UserClaims.Add(new UserClaim
            {
                UserId = userId,
                ClaimType = "role",
                ClaimValue = "admin"
            });
        }

        await context.SaveChangesAsync();
    }
}
