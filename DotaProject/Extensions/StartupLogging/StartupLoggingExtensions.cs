using Microsoft.Data.SqlClient;
using Serilog;

namespace DotaProject.Extensions.StartupLogging;

public static class StartupLoggingExtensions
{
    public static void LoggingStartup(this WebApplication app, IConfiguration configuration)
    {
        
        Log.Information("Loaded all environment variables.");

        
        var connectionString = configuration["ConnectionStrings:PlayerDbConnection"];
        try
        {
            using var connection = new SqlConnection(connectionString);
            connection.Open();
            Log.Information("Connection to PlayerDb authenticated.");
        }
        catch (Exception ex)
        {
            Log.Error($"Failed to connect to PlayerDb: {ex.Message}");
        }
    }
}