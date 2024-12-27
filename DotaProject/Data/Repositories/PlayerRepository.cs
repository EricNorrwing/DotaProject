using DotaProject.Data.Repositories.DbContexts;
using DotaProject.Data.Repositories.Interfaces;
using DotaProject.Models;
using Microsoft.EntityFrameworkCore;

namespace DotaProject.Data.Repositories;

public class PlayerRepository(PlayerDbContext context): IPlayerRepository
{
    public async Task<IEnumerable<Player>> GetAllPlayersAsync()
    {
        return await context.Players.ToListAsync();
    }

    public async Task<Player?> GetPlayerByIdAsync(int id)
    {
        return await context.Players.FindAsync(id);
    }

    public async Task<Player> AddPlayerAsync(Player player)
    {
        context.Players.Add(player);
        await context.SaveChangesAsync();
        return player;
    }

    public async Task<Player?> UpdatePlayerAsync(int id, Player updatedPlayer)
    {
        var player = await context.Players.FindAsync(id);
        if (player == null) return null;

        player.PlayerIgn = updatedPlayer.PlayerIgn;
        player.CurrentRoles = updatedPlayer.CurrentRoles;
        player.DateofBirth = updatedPlayer.DateofBirth;
        player.Earnings = updatedPlayer.Earnings;
        player.Nationality = updatedPlayer.Nationality;
        player.PlayerName = updatedPlayer.PlayerName;
        player.Region = updatedPlayer.Region;
        player.Url = updatedPlayer.Url;
        player.YearsActive = updatedPlayer.YearsActive;

        await context.SaveChangesAsync();
        return player;
    }

    public async Task<bool> DeletePlayerAsync(int id)
    {
        var player = await context.Players.FindAsync(id);
        if (player == null) return false;

        context.Players.Remove(player);
        await context.SaveChangesAsync();
        return true;
    
    }
}