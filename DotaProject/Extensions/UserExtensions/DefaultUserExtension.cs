using System.Security.Claims;
using DotaProject.Identity;
using DotaProject.Models;

namespace DotaProject.Extensions.UserExtensions;

public static class UserServiceExtensions
{
    public static IServiceCollection AddInMemoryUser(this IServiceCollection services)
    {
        var inMemoryUser = new User(
            Username: "EN",
            Password: "any12345",
            Claims:
            [
                new Claim(IdentityPolicyConstants.AdminUserClaimName, "true")
            ]
        );

        services.AddSingleton(inMemoryUser);

        return services;
    }
}