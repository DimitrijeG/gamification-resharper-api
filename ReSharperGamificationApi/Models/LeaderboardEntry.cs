namespace ReSharperGamificationApi.Models;

public class LeaderboardEntry
{
    public int Position { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public double Points { get; set; }
}