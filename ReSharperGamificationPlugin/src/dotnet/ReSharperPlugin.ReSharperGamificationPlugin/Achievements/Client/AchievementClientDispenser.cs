using JetBrains.Annotations;
using JetBrains.Application;
using JetBrains.Lifetimes;
using JetBrains.Threading;

namespace ReSharperPlugin.ReSharperGamificationPlugin.Achievements.Client;

[ShellComponent]
public class AchievementClientDispenser
{
  private readonly IAchievementClient myClient;

  public AchievementClientDispenser(
    Lifetime lifetime,
    IAchievementClient client,
    AchievementNotifier notifier)
  {
    myClient = client;
    notifier.OnNewAchievement.Advise(lifetime, PostAchievement);
  }

  private void PostAchievement(AchievementNotification notification)
  {
    try
    {
      foreach (var goal in notification.UnlockedGoals)
      {
        myClient.PostAchievement(goal, 1.0).NoAwait(); // achievement progress full
      }
    }
    catch (GamificationClientConnectionException)
    {
      // todo: handle retry queue
    }
  }
}