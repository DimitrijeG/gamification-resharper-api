using System.Collections.Generic;
using JetBrains.Annotations;
using JetBrains.Application;
using JetBrains.Application.ActivityTrackingNew;
using JetBrains.Application.Parts;
using ReSharperPlugin.ReSharperGamificationPlugin.Resources;

namespace ReSharperPlugin.ReSharperGamificationPlugin.Achievements.Refactoring;

[ShellComponent(Instantiation.DemandAnyThreadSafe)]
public class RefactoringAchievement(RefactoringProgress progress) : IAchievement
{
  public string Group => IGamificationStatisticsTracking.Group.Refactoring;
  public string /* Localized */ ShortName => Strings.RefactoringAchievementShort_Text;
  public IProgress Progress => progress;

  public IDictionary<long, Goal> Goals => new Dictionary<long, Goal>
  {
    { 1L, new Goal(string.Format(Strings.FormattingAchievementGoal_Text, 50), 50) },
    { 2L, new Goal(string.Format(Strings.FormattingAchievementGoal_Text, 75), 75) },
    { 3L, new Goal(string.Format(Strings.FormattingAchievementGoal_Text, 100), 100) }
  };
}