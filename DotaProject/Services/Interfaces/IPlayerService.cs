using DotaProject.Models;

namespace DotaProject.Services.Interfaces;

public interface IPlayerService
{
    Task<IEnumerable<Player>> GetAllPlayersAsync();
    Task<Player?> GetPlayerByIdAsync(int id);
    Task<Player> AddPlayerAsync(Player player);
    Task<Player?> UpdatePlayerAsync(int id, Player updatedPlayer);
    Task<bool> DeletePlayerAsync(int id);
}