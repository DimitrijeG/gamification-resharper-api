using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using JetBrains.Application;
using JetBrains.UsageStatistics;

namespace ReSharperPlugin.ReSharperGamificationPlugin.UsageStatistics;

[ShellComponent]
public class UsageStatisticsActivityTracking(ActivityTracking activityTracking) : IUsageStatistics
{
  public const string GroupPrefix = "Gamification";

  public void IncrementCounter(string activityGroup, string activityId, int count)
  {
    activityTracking.TrackActivity(GroupPrefix + activityGroup, activityId, count);
  }

  public void GetCountersByGroup(string activityGroup, Action<ICollection<int>> handler)
  {
    activityTracking.GetCurrentItems(GroupPrefix + activityGroup, records =>
      handler(records.Select(record => record.Counter).ToList()));
  }
}