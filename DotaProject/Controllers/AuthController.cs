using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using DotaProject.Data.FileReaders;
using DotaProject.Models;
using DotaProject.Security;
using DotaProject.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Serilog;

namespace DotaProject.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController(IConfiguration configuration, IUserService userService) : ControllerBase
    {
        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            if (request == null || string.IsNullOrWhiteSpace(request.Username) || string.IsNullOrWhiteSpace(request.Password))
            {
                Log.Warning("Login failed: Request is invalid.");
                return BadRequest(new { message = "Username and password are required." });
            }

            var user = await userService.GetUserByUsernameAsync(request.Username);

            if (user == null)
            {
                Log.Warning($"Login failed: User '{request.Username}' not found.");
                return Unauthorized(new { message = "Invalid username or password." });
            }

            Log.Information($"User found: {user.Username}. Stored hash: {user.Password}");

            if (!PasswordEncryptor.VerifyPassword(request.Password, user.Password))
            {
                Log.Warning($"Login failed: Password mismatch for user '{request.Username}'.");
                return Unauthorized(new { message = "Invalid username or password." });
            }

            if (user.Claims == null || !user.Claims.Any())
            {
                Log.Warning($"Login failed: User '{request.Username}' has no claims assigned.");
                return Unauthorized(new { message = "User has no claims assigned." });
            }

            var roleClaim = user.Claims.FirstOrDefault(c => c.ClaimType == "role")?.ClaimValue;
            if (string.IsNullOrWhiteSpace(roleClaim))
            {
                Log.Warning($"Login failed: User '{request.Username}' has no valid role claim.");
                return Unauthorized(new { message = "User has no role assigned." });
            }

            var privateKeyPath = Path.Combine(Directory.GetCurrentDirectory(), "Keys", "private_key.pem");
            if (!System.IO.File.Exists(privateKeyPath))
            {
                Log.Error($"Private key not found at {privateKeyPath}. Cannot generate JWT token.");
                return StatusCode(500, new { message = "Internal server error: Unable to generate token." });
            }

            var token = GenerateJwtToken(user.Username, roleClaim, privateKeyPath);

            Log.Information($"Login successful: Token generated for user '{request.Username}'.");
            return Ok(new { token });
        }

        private string GenerateJwtToken(string username, string role, string privateKeyPath)
        {
            var rsa = RsaKey.GetPrivateKey(privateKeyPath);

            var signingCredentials = new SigningCredentials(new RsaSecurityKey(rsa), SecurityAlgorithms.RsaSha256);

            var claims = new[]
            {
                new Claim(ClaimTypes.Role, role),
                new Claim(JwtRegisteredClaimNames.Sub, username),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddHours(1),
                SigningCredentials = signingCredentials,
                Issuer = configuration["JwtSettings:Issuer"],
                Audience = configuration["JwtSettings:Audience"]
            };
            

            var tokenHandler = new JwtSecurityTokenHandler();
            var securityToken = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(securityToken);
        }

    }

    public class LoginRequest
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
