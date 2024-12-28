using System.Net.Http.Json;
using DotaProject.Models;
using DotaProject.Models.Dto;
using DotaProject.Services.Interfaces;

namespace DotaProject.Services
{
    public class ScraperService : IScraperService
    {
        private readonly HttpClient _httpClient;

        public ScraperService(HttpClient httpClient)
        {
            
            httpClient.BaseAddress = new Uri("http://localhost:3000"); 
            _httpClient = httpClient;
        }

        public async Task<PlayerDto> ScrapePlayerAsync(string url)
        {
            // Create the request payload
            var requestPayload = new { Url = url };

            // Send a POST request to the scraper service
            var response = await _httpClient.PostAsJsonAsync("/scrape", requestPayload);

            // Check if the response is successful
            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"Failed to scrape player data. HTTP Status: {response.StatusCode}");
            }

            // Deserialize the response into a PlayerDto
            var playerData = await response.Content.ReadFromJsonAsync<PlayerDto>();

            // Throw an exception if deserialization fails
            if (playerData == null)
            {
                throw new Exception("Failed to deserialize player data");
            }

            // Return the PlayerDto object
            return playerData;
        }

    }
}