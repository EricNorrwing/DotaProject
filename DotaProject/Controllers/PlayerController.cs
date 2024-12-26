using Microsoft.AspNetCore.Mvc;
using DotaProject.Models;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;

namespace DotaProject.Controllers;

[ApiController]
[Route("api/[controller]")] // This sets the route to /api/playera
public class PlayerController : Controller
{
    // A sample list of players (mock data)
    // private static readonly List<Player> Players = new List<Player>
    // {
    //     new Player
    //     {
    //         Id = 1,
    //         PlayerIgn = "Eric",
    //         CurrentRoles =  [ "Carry", "Hard Support" ],
    //         DateofBirth = new DateTime(1992, 10, 20),
    //         Earnings = 100,
    //         Nationality = "Sweden",
    //         PlayerName = "Uldin",
    //         Region = "WEU",
    //         Url = "uldin.se",
    //         YearsActive = "1992-2024"
    //     },
    //     new Player
    //     {
    //         Id = 2,
    //         PlayerIgn = "DotaKing",
    //         CurrentRoles = [ "Carry", "Hard Support" ],
    //         DateofBirth = new DateTime(1995, 5, 15),
    //         Earnings = 200,
    //         Nationality = "Germany",
    //         PlayerName = "Karl",
    //         Region = "EU",
    //         Url = "dotaking.gg",
    //         YearsActive = "2010-2024"
    //     }
    // };

    // GET: api/player
    [HttpGet]
    [Authorize(Policy = "admin")]
    public IActionResult GetAllPlayers()
    {
        return Ok(Players); // Return the list of players as JSON
    }

   
}

