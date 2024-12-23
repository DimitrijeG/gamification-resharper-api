using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using JetBrains.Annotations;
using JetBrains.Application;
using JetBrains.Lifetimes;
using JetBrains.Util;
using JetBrains.Util.Logging;

namespace ReSharperPlugin.ReSharperGamificationPlugin.Achievements.Client;

public interface IAchievementClient
{
  public Task PostAchievement(long goalId, double progress);
}

[ShellComponent]
public class AchievementClient(
  Lifetime lifetime,
  ILogger logger,
  IGamificationClient client,
  IJbaProvider provider) : IAchievementClient
{
  private const string POST_URI = "api/v1/achievements";

  public async Task PostAchievement(long goalId, double progress)
  {
    Logger.GetLogger<AchievementClient>().Verbose("WOOOF WOOF");
    var authToken = await provider.GetAuthToken(lifetime);
    var accessToken = await provider.GetAccessToken(lifetime);
    var request = new AchievementDto(goalId, progress, accessToken);

    await client.SendAndReadResponse<AchievementDto, object>(
      HttpMethod.Post, POST_URI, request, lifetime.ToCancellationToken(), authToken);

    logger.Verbose("Achievement successfully posted");
  }
}