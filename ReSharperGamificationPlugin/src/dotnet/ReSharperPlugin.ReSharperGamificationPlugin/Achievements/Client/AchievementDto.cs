using System.Collections.Generic;
using Newtonsoft.Json;

namespace ReSharperPlugin.ReSharperGamificationPlugin.Achievements.Client;

public class AchievementDto(long goalId, double progress, string accessToken)
{
  [JsonProperty("goalId")] public long GoalId { get; } = goalId;

  [JsonProperty("progress")] public double Progress { get; } = progress;
  
  [JsonProperty("accessToken")] public string AccessToken { get; } = accessToken;
}