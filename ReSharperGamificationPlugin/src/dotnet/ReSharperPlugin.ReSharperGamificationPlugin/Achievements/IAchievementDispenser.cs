using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using JetBrains.Application;
using JetBrains.Util;
using ReSharperPlugin.ReSharperGamificationPlugin.Achievements.Storage;

namespace ReSharperPlugin.ReSharperGamificationPlugin.Achievements;

public interface IAchievementDispenser
{
  public void Calculate(IAchievement achievement, IEnumerable<int> counters);
}

[ShellComponent]
public class AchievementDispenser(
  ILogger logger,
  IAchievementDbManager achievementStorage,
  AchievementNotifier achievementNotifier) : IAchievementDispenser
{
  public void Calculate(IAchievement achievement, IEnumerable<int> counters)
  {
    achievementStorage.WithDb(db =>
    {
      if (!db.IsOperational) return;

      var grades = CalculateGrades(achievement, counters);
      var unlocked = UnlockAchievements(achievement, grades, db);
      if (unlocked.IsEmpty()) return;

      var notification = new AchievementNotification(achievement, unlocked);
      achievementNotifier.OnNewAchievement.Fire(notification);
    });
  }

  private static IEnumerable<long> CalculateGrades(IAchievement achievement, IEnumerable<int> counters)
  {
    var progress = achievement.Progress.Calculate(counters);
    return achievement.Goals
      .OrderBy(pair => pair.Value.Total)
      .Where(pair => progress >= pair.Value.Total)
      .Select(pair => pair.Key);
  }

  private ICollection<long> UnlockAchievements(
    IAchievement achievement, IEnumerable<long> goals, IAchievementDb db)
  {
    var unlocked = goals
      .Where(goal => !db.Contains(achievement.Group, goal.ToString()))
      .AsCollection();

    foreach (var goal in unlocked)
    {
      var id = db.Create(achievement.Group, goal.ToString());
      logger.Verbose($"Unlocked achievement: {achievement.Group} {goal} ({id})");
    }

    return unlocked;
  }
}