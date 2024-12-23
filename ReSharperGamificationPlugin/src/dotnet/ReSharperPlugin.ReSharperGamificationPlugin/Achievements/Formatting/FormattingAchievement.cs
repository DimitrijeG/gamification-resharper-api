using System.Collections.Generic;
using JetBrains.Annotations;
using JetBrains.Application;
using JetBrains.Application.ActivityTrackingNew;
using JetBrains.Application.Parts;
using ReSharperPlugin.ReSharperGamificationPlugin.Resources;

namespace ReSharperPlugin.ReSharperGamificationPlugin.Achievements.Formatting;

[ShellComponent(Instantiation.DemandAnyThreadSafe)]
public class FormattingAchievement(FormattingProgress progress) : IAchievement
{
  public string Group => IGamificationStatisticsTracking.Group.Formatting;
  public string /* Localized */ ShortName => Strings.FormattingAchievementShort_Text;
  public IProgress Progress => progress;

  public IDictionary<long, Goal> Goals => new Dictionary<long, Goal>
  {
    { 4L, new Goal(string.Format(Strings.FormattingAchievementGoal_Text, 5), 5) },
    { 5L, new Goal(string.Format(Strings.FormattingAchievementGoal_Text, 8), 8) },
    { 6L, new Goal(string.Format(Strings.FormattingAchievementGoal_Text, 10), 10) }
  };
}