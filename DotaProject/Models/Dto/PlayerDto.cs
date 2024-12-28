namespace DotaProject.Models.Dto;

public class PlayerDto
{
    public string InGameName { get; set; }
    public string Name { get; set; }
    public string Nationality { get; set; }
    public int Earnings { get; set; }
    public string DateOfBirth { get; set; }
    public string Region { get; set; }
    public string YearsActive { get; set; }
    public string[] CurrentRoles { get; set; }
    public string Team { get; set; }
    public string Url { get; set; }
}