using System.Linq;
using System.Threading.Tasks;
using JetBrains.Application.Parts;
using JetBrains.Application.Threading;
using JetBrains.Application.UI.Icons.CommonThemedIcons;
using JetBrains.DataFlow;
using JetBrains.IDE.FeatureSuggestion;
using JetBrains.Lifetimes;
using JetBrains.ProjectModel;
using ReSharperPlugin.ReSharperGamificationPlugin.Achievements.UIInteractive;
using ReSharperPlugin.ReSharperGamificationPlugin.Resources;

namespace ReSharperPlugin.ReSharperGamificationPlugin.Achievements;

[SolutionComponent(Instantiation.DemandAnyThreadUnsafe)]
public class AchievementFeatureSuggester(
  IThreading threading,
  AchievementNotifier achievementNotifier,
  AchievementsDialogAction dialogAction,
  IFeatureSuggestionNotificationProvider notificationProvider,
  IJbaProvider jbaProvider,
  IGamificationServiceUrlProvider serviceUrlProvider) : IFeatureSuggester
{
  public IProperty<IFeatureSuggestion> Subscribe(Lifetime lifetime, FeatureSuggestionMode mode)
  {
    var suggestion = new Property<IFeatureSuggestion>("AchievementFeatureSuggester.Suggestion");

    achievementNotifier.OnNewAchievement.Advise(lifetime, notification =>
    {
      var name = notification.Achievement.ShortName;
      var lastGoal = notification.UnlockedGoals.Last();
      ShowAchievement(suggestion, lifetime, name, notification.Achievement.Goals[lastGoal].Name);
    });

    return suggestion;
  }

  public IFeatureSuggestionAspect[] Aspects { get; } =
  [
    IgnoreFeatureSuggestionModeAspect.Instance,
    NoSuggestionsIntervalAspect.Instance
  ];

  private async void ShowAchievement(
    Property<IFeatureSuggestion> suggestion, Lifetime lifetime, string name, string grade)
  {
    var notification = notificationProvider.TryCreate(lifetime);
    if (notification == null) return;

    notification.AcceptText = Strings.ShowAchievements_Text;
    notification.Accepted.Advise(lifetime, dialogAction.Execute);
    notification.SuggestionMessage = $"You achieved {grade} in {name}!";
    notification.IconId = CommonThemedIcons.Bulb.Id; // todo: change to achievement icon
    notification.HelpUrl = await GetLeagueUrl(lifetime);
    notification.LearnMoreText = "Show leaderboard";

    threading.ExecuteOrQueueEx(lifetime, "Show Achievement explanation", () =>
    {
      notification.Lifetime.TryOnTermination(() => suggestion.SetValue(null));
      suggestion.SetValue(notification);
    });
  }

  private async Task<string> GetLeagueUrl(Lifetime lifetime)
  {
    return serviceUrlProvider.ServiceUrl + "league/" + await jbaProvider.GetAccessToken(lifetime);
  }
}