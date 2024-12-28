using System.Diagnostics;
using System.Text;
using System.Text.Json;

namespace DotaProject.Clients.ScraperClient;

public class ScraperClient
{
    private const string ScraperUrl = "http://localhost:3000"; 
    private static Process? _scraperProcess;

    public ScraperClient()
    {
        StartScraperServer();
    }

    private void StartScraperServer()
    {
        if (_scraperProcess == null || _scraperProcess.HasExited)
        {
            _scraperProcess = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = "node", 
                    Arguments = "ScraperProject/Scraper/src/index.ts", 
                    WorkingDirectory = @"ScraperProject/ScraperProject.csproj",
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    CreateNoWindow = true
                }
            };

            _scraperProcess.Start();
        }
    }

    public async Task<string> ScrapePlayerAsync(string url)
    {
        using var client = new HttpClient();
        var payload = JsonSerializer.Serialize(new { url });
        var content = new StringContent(payload, Encoding.UTF8, "application/json");

        var response = await client.PostAsync($"{ScraperUrl}/scrape", content);
        response.EnsureSuccessStatusCode();

        return await response.Content.ReadAsStringAsync();
    }

    ~ScraperClient()
    {
        
        if (_scraperProcess is { HasExited: false })
        {
            _scraperProcess.Kill();
            _scraperProcess.Dispose();
        }
    }
}