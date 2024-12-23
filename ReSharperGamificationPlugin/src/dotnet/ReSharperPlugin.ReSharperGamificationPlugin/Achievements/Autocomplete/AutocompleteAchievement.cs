using System.Collections.Generic;
using JetBrains.Annotations;
using JetBrains.Application;
using JetBrains.Application.ActivityTrackingNew;
using JetBrains.Application.Parts;
using ReSharperPlugin.ReSharperGamificationPlugin.Resources;

namespace ReSharperPlugin.ReSharperGamificationPlugin.Achievements.Autocomplete;

[ShellComponent(Instantiation.DemandAnyThreadSafe)]
public class AutocompleteAchievement(AutocompletionProgress progress) : IAchievement
{
  public string Group => IGamificationStatisticsTracking.Group.Autocompletion;
  public string /* Localized */ ShortName => Strings.AutocompleteAchievementShort_Text;
  public IProgress Progress => progress;

  public IDictionary<long, Goal> Goals => new Dictionary<long, Goal>
  {
    { 7L, new Goal(string.Format(Strings.AutocompleteAchievementGoal_Text, 60), 60) },
    { 8L, new Goal(string.Format(Strings.AutocompleteAchievementGoal_Text, 80), 80) },
    { 9L, new Goal(string.Format(Strings.AutocompleteAchievementGoal_Text, 100), 100) }
  };
}