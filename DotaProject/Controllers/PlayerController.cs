using Microsoft.AspNetCore.Mvc;
using DotaProject.Models;
using DotaProject.Services;
using DotaProject.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Data.SqlClient;

namespace DotaProject.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PlayerController : ControllerBase
    {
        private readonly IPlayerService _service;

        public PlayerController(IPlayerService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllPlayers()
        {
            var players = await _service.GetAllPlayersAsync();
            return Ok(players);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetPlayerById(int id)
        {
            var player = await _service.GetPlayerByIdAsync(id);
            if (player == null)
            {
                return NotFound(new { Message = "Player not found." });
            }
            return Ok(player);
        }

        [HttpPost]
        public async Task<IActionResult> CreatePlayer([FromBody] Player newPlayer)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var player = await _service.AddPlayerAsync(newPlayer);
            return CreatedAtAction(nameof(GetPlayerById), new { id = player.Id }, player);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdatePlayer(int id, [FromBody] Player updatedPlayer)
        {
            var player = await _service.UpdatePlayerAsync(id, updatedPlayer);
            if (player == null)
            {
                return NotFound(new { Message = "Player not found." });
            }

            return NoContent();
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeletePlayer(int id)
        {
            var success = await _service.DeletePlayerAsync(id);
            if (!success)
            {
                return NotFound(new { Message = "Player not found." });
            }

            return NoContent();
        }
        
       
    }
}
