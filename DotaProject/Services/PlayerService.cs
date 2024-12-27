using DotaProject.Data.Repositories.Interfaces;
using DotaProject.Models;
using DotaProject.Services.Interfaces;

namespace DotaProject.Services;

public class PlayerService(IPlayerRepository repository): IPlayerService
{
    public async Task<IEnumerable<Player>> GetAllPlayersAsync()
    {
        return await repository.GetAllPlayersAsync();
    }

    public async Task<Player?> GetPlayerByIdAsync(int id)
    {
        return await repository.GetPlayerByIdAsync(id);
    }

    public async Task<Player> AddPlayerAsync(Player player)
    {
        return await repository.AddPlayerAsync(player);
    }

    public async Task<Player?> UpdatePlayerAsync(int id, Player updatedPlayer)
    {
        return await repository.UpdatePlayerAsync(id, updatedPlayer);
    }

    public async Task<bool> DeletePlayerAsync(int id)
    {
        return await repository.DeletePlayerAsync(id);
    }
}