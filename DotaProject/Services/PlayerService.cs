using DotaProject.Data.Repositories.Interfaces;
using DotaProject.Models;
using DotaProject.Services.Interfaces;

namespace DotaProject.Services;

public class PlayerService: IPlayerService
{
    private readonly IPlayerRepository _repository;

    public PlayerService(IPlayerRepository repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<Player>> GetAllPlayersAsync()
    {
        return await _repository.GetAllPlayersAsync();
    }

    public async Task<Player?> GetPlayerByIdAsync(int id)
    {
        return await _repository.GetPlayerByIdAsync(id);
    }

    public async Task<Player> AddPlayerAsync(Player player)
    {
        return await _repository.AddPlayerAsync(player);
    }

    public async Task<Player?> UpdatePlayerAsync(int id, Player updatedPlayer)
    {
        return await _repository.UpdatePlayerAsync(id, updatedPlayer);
    }

    public async Task<bool> DeletePlayerAsync(int id)
    {
        return await _repository.DeletePlayerAsync(id);
    }
}