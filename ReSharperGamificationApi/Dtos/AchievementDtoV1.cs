using System.Text.Json.Serialization;

namespace ReSharperGamificationApi.Dtos;

public class AchievementDtoV1
{
    [JsonPropertyName("id")] public long Id { get; set; }

    [JsonPropertyName("user")] public UserDtoV1 User { get; set; } = null!;

    [JsonPropertyName("grade")] public GradeDtoV1 Grade { get; set; } = null!;
}

public class GradeDtoV1
{
    [JsonPropertyName("id")] public long Id { get; set; }

    [JsonPropertyName("name")] public string Name { get; set; } = string.Empty;

    [JsonPropertyName("points")] public double Points { get; set; }

    [JsonPropertyName("group")] public GroupDtoV1 Group { get; set; } = null!;
}

public class GroupDtoV1
{
    [JsonPropertyName("id")] public long Id { get; set; }

    [JsonPropertyName("name")] public string Name { get; set; } = string.Empty;
}