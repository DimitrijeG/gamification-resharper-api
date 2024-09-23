using System.Text.Json.Serialization;

namespace ReSharperGamificationApi.Dto;

public class AchievementDtoV1
{
    [JsonPropertyName("id")]
    public long Id { get; set; }
    [JsonPropertyName("user")]
    public UserDtoV1 User { get; set; } = null!;
    [JsonPropertyName("grade")]
    public GradeDtoV1 Grade { get; set; } = null!;
}