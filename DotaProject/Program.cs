using System.Text;
using DotaProject.Data;
using DotaProject.Data.FileReaders;
using DotaProject.Data.Repositories;
using DotaProject.Data.Repositories.Interfaces;
using DotaProject.Extensions.StartupLogging;
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

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Debug() 
    .WriteTo.Console() 
    .WriteTo.File("logs/log-.txt", rollingInterval: RollingInterval.Day) 
    .ReadFrom.Configuration(builder.Configuration)
    .CreateLogger();

builder.Host.UseSerilog();


IConfiguration config = builder.Configuration;


builder.Services.AddControllers();

builder.Services
    .AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = config["JwtSettings:Issuer"],
            ValidateAudience = true,
            ValidAudience = config["JwtSettings:Audience"],
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(config["JwtSettings:Key"])),
            ValidateLifetime = true
        };
    });
builder.Services.AddAuthorization(auth =>
{
    auth.AddPolicy(IdentityPolicyConstants.AdminUserPolicyName, policy => policy.RequireClaim(IdentityPolicyConstants.AdminUserClaimName, "true"));
});

builder.Services.AddDbContext<PlayerDbContext>(options => 
    options.UseSqlServer(config.GetConnectionString("PlayerDbConnection")));

builder.Services.AddDbContext<AuthDbContext>(options =>
    options.UseSqlServer(config.GetConnectionString("AuthDbConnection")));

builder.Services.AddScoped<IPlayerRepository, PlayerRepository>();
builder.Services.AddScoped<IPlayerService, PlayerService>();


var app = builder.Build();


app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.LoggingStartup(builder.Configuration);
app.MapControllers();

app.Run();