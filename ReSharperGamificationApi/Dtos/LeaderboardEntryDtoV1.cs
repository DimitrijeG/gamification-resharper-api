using System.Text.Json.Serialization;

namespace ReSharperGamificationApi.Dtos;

public class LeaderboardEntryDtoV1
{
    [JsonPropertyName("position")] public int Position { get; set; }

    [JsonPropertyName("firstName")] public string FirstName { get; set; } = string.Empty;

    [JsonPropertyName("lastName")] public string LastName { get; set; } = string.Empty;

    [JsonPropertyName("points")] public double Points { get; set; }
}