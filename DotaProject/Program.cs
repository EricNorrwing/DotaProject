using System.Text;
using DotaProject.Data;
using DotaProject.Extensions.UserExtensions;
using DotaProject.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Serilog;

var builder = WebApplication.CreateBuilder(args);


Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .CreateLogger();

builder.Host.UseSerilog();


IConfiguration config = builder.Configuration;

builder.Services.AddControllers();
builder.Services.AddInMemoryUser();

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

var app = builder.Build();


app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();
