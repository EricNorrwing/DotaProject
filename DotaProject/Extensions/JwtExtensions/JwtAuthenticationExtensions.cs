using DotaProject.Data.FileReaders;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

namespace DotaProject.Extensions.JwtExtensions
{
    public static class JwtAuthenticationExtensions
    {
        public static void AddJwtAuthentication(this IServiceCollection services, IConfiguration configuration)
        {
            // Load RSA keys
            var privateKeyPath = Path.Combine(Directory.GetCurrentDirectory(), "Keys", "private_key.pem");
            var publicKeyPath = Path.Combine(Directory.GetCurrentDirectory(), "Keys", "public_key.pem");

            var rsaPublicKey = RsaKey.GetPublicKey(publicKeyPath);

            // Configure JWT authentication
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidIssuer = configuration["JwtSettings:Issuer"],

                    ValidateAudience = true,
                    ValidAudience = configuration["JwtSettings:Audience"],

                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new RsaSecurityKey(rsaPublicKey),

                    ValidateLifetime = true
                };
            });
        }
    }
}