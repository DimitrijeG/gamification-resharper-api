using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using JetBrains.Application;
using JetBrains.Application.Threading;
using JetBrains.Lifetimes;
using JetBrains.Util;
using ReSharperPlugin.ReSharperGamificationPlugin.Achievements;
using ReSharperPlugin.ReSharperGamificationPlugin.UsageStatistics;

namespace ReSharperPlugin.ReSharperGamificationPlugin;

[ShellComponent]
public class GamificationStatisticsTracking(
  Lifetime lifetime,
  ILogger logger,
  IThreading threading,
  IUsageStatistics usageStatistics,
  IEnumerable<IAchievement> achievements,
  IAchievementDispenser dispenser) : JetBrains.Application.ActivityTrackingNew.GamificationStatisticsTracking
{
  public override void TrackActivity(string activityGroup, string activityId, int count)
  {
    logger.Verbose($"Tracked: {activityGroup}-{activityId} ({count})");

    usageStatistics.IncrementCounter(activityGroup, activityId, count);
    usageStatistics.GetCountersByGroup(activityGroup, counters =>
    {
      threading.Queue(lifetime, "Calculate achievements for activity", () =>
      {
        foreach (var achievement in achievements.Where(achievement => achievement.Group.Equals(activityGroup)))
          dispenser.Calculate(achievement, counters);
      });
    });
  }
}