using System.Text.Json.Serialization;

namespace ReSharperGamificationApi.Dtos;

[method: JsonConstructor]
public class AchievementRequestDtoV1(long goalId, double progress, string accessToken)
{
    [JsonPropertyName("goalId")] public long GoalId { get; } = goalId;

    [JsonPropertyName("progress")] public double Progress { get; } = progress;

    [JsonPropertyName("accessToken")] public string AccessToken { get; } = accessToken;
}