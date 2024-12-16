using System.Text.Json.Serialization;

namespace ReSharperGamificationApi.Models;

public class LeaderboardEntry
{
    [JsonPropertyName("position")] public int Position { get; set; }

    [JsonPropertyName("firstName")] public string FirstName { get; set; } = string.Empty;

    [JsonPropertyName("lastName")] public string LastName { get; set; } = string.Empty;

    [JsonPropertyName("points")] public double Points { get; set; }
}