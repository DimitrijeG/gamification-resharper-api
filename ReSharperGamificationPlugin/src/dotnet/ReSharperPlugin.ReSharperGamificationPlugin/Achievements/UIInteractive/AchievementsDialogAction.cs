using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JetBrains.Annotations;
using JetBrains.Application.Parts;
using JetBrains.Application.Threading;
using JetBrains.Lifetimes;
using JetBrains.ProjectModel;
using JetBrains.Util;
using ReSharperPlugin.ReSharperGamificationPlugin.Achievements.Storage;
using ReSharperPlugin.ReSharperGamificationPlugin.UsageStatistics;

namespace ReSharperPlugin.ReSharperGamificationPlugin.Achievements.UIInteractive;

public interface IAchievementsDialogProvider
{
  void ShowDialog(LifetimeDefinition ld, ISolution solution, AchievementsViewModel viewModel);
}

[SolutionComponent(Instantiation.DemandAnyThreadSafe)]
public class AchievementsDialogAction(
  ISolution solution,
  IAchievementsDialogProvider dialogProvider,
  IAchievementDbManager achievementStorage,
  IUsageStatistics usageStatistics,
  IGradeStyleProvider gradeStyleProvider,
  IEnumerable<IAchievement> achievements)
{
  private ICollection<IAchievement> Achievements { get; } = achievements.AsCollection();

  public void Execute()
  {
    var ld = new LifetimeDefinition(solution.GetSolutionLifetimes().UntilSolutionCloseLifetime);
    solution.GetComponent<IShellLocks>().QueueReadLock(ld.Lifetime, "ShowAchievementsDialog", ExecuteAsync);
    return;

    async void ExecuteAsync()
    {
      var achievements = await GetAllAchievements();
      var viewModel = new AchievementsViewModel(achievements);
      dialogProvider.ShowDialog(ld, solution, viewModel);
    }
  }


  private async Task<ICollection<AchievementItemViewModel>> GetAllAchievements()
  {
    var tcs = new TaskCompletionSource<ICollection<AchievementItemViewModel>>();
    achievementStorage.WithDb(CalculateAchievements);
    return await tcs.Task;

    async void CalculateAchievements(IAchievementDb db)
    {
      var viewModels = await Task.WhenAll(
        Achievements.Select(achievement => CreateAchievementViewModel(achievement, db)));
      tcs.SetResult(viewModels);
    }
  }

  private async Task<AchievementItemViewModel> CreateAchievementViewModel(IAchievement achievement, IAchievementDb db)
  {
    var grades = await Task.WhenAll(
      achievement.Goals
        .OrderBy(pair => pair.Value.Total)
        .Select(pair => CreateGradeViewModel(achievement, pair.Key, db.Contains(achievement.Group, pair.Key.ToString())))
    );

    return new AchievementItemViewModel(achievement.Group, grades);
  }

  private async Task<GradeItemViewModel> CreateGradeViewModel(IAchievement achievement, long goalId, bool unlocked)
  {
    var goal = achievement.Goals[goalId];
    var grade = new GradeItemViewModel(
      goal.Name, goal.Total, unlocked,
      gradeStyleProvider.GetIcon(achievement.Group),
      gradeStyleProvider.GetColor(achievement.Group, goalId));

    if (unlocked) return grade;

    var tcs = new TaskCompletionSource<GradeItemViewModel>();
    usageStatistics.GetCountersByGroup(achievement.Group, counters =>
    {
      grade.SetCounted(achievement.Progress.Calculate(counters));
      tcs.SetResult(grade);
    });
    return await tcs.Task;
  }
}