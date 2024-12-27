using Microsoft.Data.SqlClient;
using Serilog;

namespace DotaProject.Extensions.UserExtensions
{
    public static class DefaultUserExtensions
    {
        public static void AddDefaultUsers(this WebApplication app, IConfiguration configuration)
        {
            var connectionString = configuration["ConnectionStrings:AuthDbConnection"];
            var sqlFilePath = Path.Combine(Directory.GetCurrentDirectory(), "Extensions", "UserExtensions", "AddDefaultUsers.sql");

            try
            {
                if (!File.Exists(sqlFilePath))
                {
                    Log.Warning($"SQL file not found at: {sqlFilePath}");
                    return;
                }

                using (var connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    var sqlScript = File.ReadAllText(sqlFilePath);

                    using (var command = new SqlCommand(sqlScript, connection))
                    {
                        command.ExecuteNonQuery();
                        Log.Information("Default users and claims added to the AuthDb database via SQL file.");
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Error($"Failed to execute SQL file for adding default users and claims: {ex.Message}");
            }
        }
    }
}