public class Player
{
    public int Id { get; set; }
    public string InGameName { get; set; } = string.Empty;
    public string PlayerName { get; set; } = string.Empty;
    public string Nationality { get; set; } = string.Empty;
    public int Earnings { get; set; }
    public string DateOfBirth { get; set; } = string.Empty; 
    public string Region { get; set; } = string.Empty;
    public string YearsActive { get; set; } = string.Empty;
    public string[] CurrentRoles { get; set; } = Array.Empty<string>();
    public string Team { get; set; } = string.Empty;
    public string Url { get; set; } = string.Empty;
}