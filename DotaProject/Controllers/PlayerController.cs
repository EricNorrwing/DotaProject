using Microsoft.AspNetCore.Mvc;
using DotaProject.Models;
using System.Collections.Generic;

namespace DotaProject.Controllers;

[ApiController]
[Route("api/[controller]")] // This sets the route to /api/playera
public class PlayerController : Controller
{
    // A sample list of players (mock data)
    private static readonly List<Player> Players = new List<Player>
    {
        new Player
        {
            Id = 1,
            PlayerIgn = "Eric",
            CurrentRoles =  [ "Carry", "Hard Support" ],
            DateofBirth = new DateTime(1992, 10, 20),
            Earnings = 100,
            Nationality = "Sweden",
            PlayerName = "Uldin",
            Region = "WEU",
            Url = "uldin.se",
            YearsActive = "1992-2024"
        },
        new Player
        {
            Id = 2,
            PlayerIgn = "DotaKing",
            CurrentRoles = [ "Carry", "Hard Support" ],
            DateofBirth = new DateTime(1995, 5, 15),
            Earnings = 200,
            Nationality = "Germany",
            PlayerName = "Karl",
            Region = "EU",
            Url = "dotaking.gg",
            YearsActive = "2010-2024"
        }
    };

    // GET: api/player
    [HttpGet]
    public IActionResult GetAllPlayers()
    {
        return Ok(Players); // Return the list of players as JSON
    }

    // GET: api/player/1
    [HttpGet("{id:int}")]
    public IActionResult GetPlayerById(int id)
    {
        var player = Players.Find(p => p.Id == id);
        if (player == null)
        {
            return NotFound(new { Message = "Player not found." });
        }
        return Ok(player); // Return the player as JSON
    }

    // POST: api/player
    [HttpPost]
    public IActionResult CreatePlayer([FromBody] Player newPlayer)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        newPlayer.Id = Players.Count + 1; // Mock ID assignment
        Players.Add(newPlayer);
        return CreatedAtAction(nameof(GetPlayerById), new { id = newPlayer.Id }, newPlayer);
    }

    // PUT: api/player/1
    [HttpPut("{id:int}")]
    public IActionResult UpdatePlayer(int id, [FromBody] Player updatedPlayer)
    {
        var player = Players.Find(p => p.Id == id);
        if (player == null)
        {
            return NotFound(new { Message = "Player not found." });
        }

        // Update player properties
        player.PlayerIgn = updatedPlayer.PlayerIgn;
        player.CurrentRoles = updatedPlayer.CurrentRoles;
        player.DateofBirth = updatedPlayer.DateofBirth;
        player.Earnings = updatedPlayer.Earnings;
        player.Nationality = updatedPlayer.Nationality;
        player.PlayerName = updatedPlayer.PlayerName;
        player.Region = updatedPlayer.Region;
        player.Url = updatedPlayer.Url;
        player.YearsActive = updatedPlayer.YearsActive;

        return NoContent(); // RESTful convention for successful updates
    }

    // DELETE: api/player/1
    [HttpDelete("{id:int}")]
    public IActionResult DeletePlayer(int id)
    {
        var player = Players.Find(p => p.Id == id);
        if (player == null)
        {
            return NotFound(new { Message = "Player not found." });
        }

        Players.Remove(player);
        return NoContent(); // RESTful convention for successful deletes
    }
}

