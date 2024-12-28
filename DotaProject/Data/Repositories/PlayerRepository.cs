using DotaProject.Data.Repositories.DbContexts;
using DotaProject.Data.Repositories.Interfaces;
using DotaProject.Models;
using Microsoft.EntityFrameworkCore;

namespace DotaProject.Data.Repositories;

public class PlayerRepository : IPlayerRepository
{
    private readonly PlayerDbContext _context;

    public PlayerRepository(PlayerDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Player>> GetAllPlayersAsync()
    {
        return await _context.Players.ToListAsync();
    }

    public async Task<Player?> GetPlayerByIdAsync(int id)
    {
        return await _context.Players.FindAsync(id);
    }

    public async Task<Player> AddPlayerAsync(Player player)
    {
        _context.Players.Add(player);
        await _context.SaveChangesAsync();
        return player;
    }

    public async Task<Player?> UpdatePlayerAsync(int id, Player updatedPlayer)
    {
        var player = await _context.Players.FindAsync(id);
        if (player == null) return null;

        
        player.InGameName = updatedPlayer.InGameName;
        player.PlayerName = updatedPlayer.PlayerName; 
        player.Nationality = updatedPlayer.Nationality;
        player.Earnings = updatedPlayer.Earnings;
        player.DateOfBirth = updatedPlayer.DateOfBirth; 
        player.Region = updatedPlayer.Region;
        player.YearsActive = updatedPlayer.YearsActive;
        player.CurrentRoles = updatedPlayer.CurrentRoles;
        player.Team = updatedPlayer.Team;
        player.Url = updatedPlayer.Url;

        // Save changes
        await _context.SaveChangesAsync();
        return player;
    }

    public async Task<bool> DeletePlayerAsync(int id)
    {
        var player = await _context.Players.FindAsync(id);
        if (player == null) return false;

        _context.Players.Remove(player);
        await _context.SaveChangesAsync();
        return true;
    }
}