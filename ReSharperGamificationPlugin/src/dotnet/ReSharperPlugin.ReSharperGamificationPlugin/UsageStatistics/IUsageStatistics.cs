using System;
using System.Collections.Generic;

namespace ReSharperPlugin.ReSharperGamificationPlugin.UsageStatistics;

public interface IUsageStatistics
{
  void IncrementCounter(string activityGroup, string activityId, int count);
  void GetCountersByGroup(string activityGroup, Action<ICollection<int>> handler);
}