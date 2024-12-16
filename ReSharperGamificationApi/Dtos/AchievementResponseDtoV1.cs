using System.Text.Json.Serialization;

namespace ReSharperGamificationApi.Dtos;

public class AchievementResponseDtoV1
{
    [JsonPropertyName("id")] public long Id { get; set; }

    [JsonPropertyName("goal")] public GoalDtoV1 Goal { get; set; } = null!;

    [JsonPropertyName("user")] public UserDtoV1 User { get; set; } = null!;

    [JsonPropertyName("progress")] public double Progress { get; set; }
}

public class GoalDtoV1
{
    [JsonPropertyName("id")] public long Id { get; set; }

    [JsonPropertyName("name")] public string Name { get; set; } = string.Empty;

    [JsonPropertyName("description")] public string Description { get; set; } = string.Empty;

    [JsonPropertyName("lifetime")] public string Lifetime { get; set; } = string.Empty;

    [JsonPropertyName("points")] public double Points { get; set; }

    [JsonPropertyName("group")] public GroupDtoV1 Group { get; set; } = null!;
}

public class GroupDtoV1
{
    [JsonPropertyName("id")] public long Id { get; set; }

    [JsonPropertyName("name")] public string Name { get; set; } = string.Empty;
}