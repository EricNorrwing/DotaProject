using System.Text.Json.Serialization;

namespace DotaProject.Models
{
    public class User
    {
        public int Id { get; set; } 
        public string Username { get; set; } 
        public string Password { get; set; } 
        
        [JsonIgnore]
        public List<UserClaim>? Claims { get; set; } = new List<UserClaim>();
    }
}