using System.Security.Claims;
using System.Text;
using DotaProject.Data;
using DotaProject.Data.FileReaders;
using DotaProject.Data.Repositories;
using DotaProject.Data.Repositories.DbContexts;
using DotaProject.Data.Repositories.Interfaces;
using DotaProject.Extensions;
using DotaProject.Extensions.JwtExtensions;
using DotaProject.Extensions.StartupLogging;
using DotaProject.Extensions.UserExtensions;
using DotaProject.Identity;
using DotaProject.Services;
using DotaProject.Services.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

//Environment variables load
var root = Directory.GetCurrentDirectory();
var dotenv = Path.Combine(root, "secrets.env");
DotEnv.Load(dotenv);
Log.Information("Loaded .env files");
builder.Configuration.AddEnvironmentVariables();



//SeriLog
Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Debug() 
    .WriteTo.Console() 
    .WriteTo.File("logs/log-.txt", rollingInterval: RollingInterval.Day)
    .ReadFrom.Configuration(builder.Configuration)
    .CreateLogger();

builder.Host.UseSerilog();


IConfiguration config = builder.Configuration;


builder.Services.AddControllers();



//JWT
builder.Services.AddJwtAuthentication(builder.Configuration);

//Authorization
builder.Services.AddAuthorization(options =>
{
    
    options.AddPolicy(IdentityPolicyConstants.AdminUserPolicyName, policy =>
        policy.RequireClaim(ClaimTypes.Role, IdentityPolicyConstants.AdminUserClaimName));
    
    options.AddPolicy(IdentityPolicyConstants.VerifiedUserPolicyName, policy =>
        policy.RequireClaim(ClaimTypes.Role, IdentityPolicyConstants.VerifiedUserClaimName));
});

//Json Serialization options
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.Preserve;
        options.JsonSerializerOptions.WriteIndented = true;
    });

//Database
builder.Services.AddDbContext<PlayerDbContext>(options => 
    options.UseSqlServer(config.GetConnectionString("PlayerDbConnection")));
builder.Services.AddDbContext<AuthDbContext>(options =>
    options.UseSqlServer(config.GetConnectionString("AuthDbConnection")));
builder.Services.AddScoped<IPlayerRepository, PlayerRepository>();
builder.Services.AddScoped<IPlayerService, PlayerService>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IUserService, UserService>();

//TODO CORS
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader();
    });
});



var app = builder.Build();
//TODO REMOVE
app.Use(async (context, next) =>
{
    if (context.User.Identity is { IsAuthenticated: true })
    {
        var claims = context.User.Claims.Select(c => $"{c.Type}={c.Value}");
        Log.Information($"Authenticated user claims: {string.Join(", ", claims)}");
    }
    else
    {
        
        Log.Warning("User is not authenticated.");
    }

    await next();
});

app.UseCors();
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.LoggingStartup(builder.Configuration);
app.AddDefaultUsers(builder.Configuration);

app.MapControllers();

app.Run();