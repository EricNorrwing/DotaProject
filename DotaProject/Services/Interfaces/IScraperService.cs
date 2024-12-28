using DotaProject.Models;
using DotaProject.Models.Dto;

namespace DotaProject.Services.Interfaces;

public interface IScraperService
{
    Task<PlayerDto> ScrapePlayerAsync(string url);
}