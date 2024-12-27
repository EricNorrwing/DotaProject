using System.Text.Json.Serialization;

namespace DotaProject.Models;

public class UserClaim
{
    public int Id { get; set; } 
    public int UserId { get; set; } 
    public string ClaimType { get; set; } 
    public string ClaimValue { get; set; } 

    
    [JsonIgnore]
    public User User { get; set; }
}