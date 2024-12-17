using System.Security.Claims;

namespace DotaProject.Models;

public record User(
    string Username,
    string Password,
    IEnumerable<Claim> Claims
);