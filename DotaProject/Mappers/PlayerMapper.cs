using DotaProject.Models;
using DotaProject.Models.Dto;

namespace DotaProject.Mappers
{
    public static class PlayerMapper
    {
        public static Player ToPlayer(PlayerDto dto)
        {
            return new Player
            {
                InGameName = dto.InGameName,
                PlayerName = dto.Name,
                Nationality = dto.Nationality,
                Earnings = dto.Earnings,
                DateOfBirth = dto.DateOfBirth, 
                Region = dto.Region,
                YearsActive = dto.YearsActive,
                CurrentRoles = dto.CurrentRoles,
                Team = dto.Team,
                Url = dto.Url
            };
        }
    }
}