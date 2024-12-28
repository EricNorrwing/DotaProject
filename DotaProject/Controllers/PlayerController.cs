using Microsoft.AspNetCore.Mvc;
using DotaProject.Models;
using DotaProject.Services.Interfaces;
using DotaProject.Extensions;


namespace DotaProject.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PlayerController : ControllerBase
    {
        private readonly IPlayerService _playerService;
        private readonly IScraperService _scraperService;

        public PlayerController(IPlayerService playerService, IScraperService scraperService)
        {
            _playerService = playerService;
            _scraperService = scraperService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllPlayers()
        {
            var players = await _playerService.GetAllPlayersAsync();
            return Ok(players);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetPlayerById(int id)
        {
            var player = await _playerService.GetPlayerByIdAsync(id);
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

            var player = await _playerService.AddPlayerAsync(newPlayer);
            return CreatedAtAction(nameof(GetPlayerById), new { id = player.Id }, player);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdatePlayer(int id, [FromBody] Player updatedPlayer)
        {
            var player = await _playerService.UpdatePlayerAsync(id, updatedPlayer);
            if (player == null)
            {
                return NotFound(new { Message = "Player not found." });
            }

            return NoContent();
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeletePlayer(int id)
        {
            var success = await _playerService.DeletePlayerAsync(id);
            if (!success)
            {
                return NotFound(new { Message = "Player not found." });
            }

            return NoContent();
        }

        [HttpPost("fetchPlayer")]
        public async Task<IActionResult> FetchPlayer([FromBody] UrlRequest request)
        {
            if (string.IsNullOrEmpty(request.Url))
            {
                return BadRequest("URL is required");
            }

            try
            {
                
                var playerDto = await _scraperService.ScrapePlayerAsync(request.Url);


                var player = PlayerDtoExtensions.ToPlayer(playerDto);

                
                var savedPlayer = await _playerService.AddPlayerAsync(player);

                
                return Ok(savedPlayer);
            }
            catch (Exception ex)
            {
                
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }

        public class UrlRequest
        {
            public string Url { get; set; }
        }
    }
}
