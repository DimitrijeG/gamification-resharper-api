using System.Collections.Generic;
using JetBrains.Application;
using JetBrains.Application.Parts;
using JetBrains.Collections.Viewable;

namespace ReSharperPlugin.ReSharperGamificationPlugin.Achievements;

[ShellComponent(Instantiation.DemandAnyThreadSafe)]
public class AchievementNotifier
{
  public Signal<AchievementNotification> OnNewAchievement { get; } = new();
}

public class AchievementNotification(IAchievement achievement, ICollection<long> unlockedGoals)
{
  public IAchievement Achievement { get; } = achievement;
  public ICollection<long> UnlockedGoals { get; } = unlockedGoals;
}