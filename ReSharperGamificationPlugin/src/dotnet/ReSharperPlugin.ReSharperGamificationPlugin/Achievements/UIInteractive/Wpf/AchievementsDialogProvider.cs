using System;
using System.Threading.Tasks;
using JetBrains.Annotations;
using JetBrains.Application.BuildScript.Application;
using JetBrains.Application.Parts;
using JetBrains.Application.StdApplicationUI;
using JetBrains.Application.Threading;
using JetBrains.Application.UI.Help;
using JetBrains.Lifetimes;
using JetBrains.ProjectModel;
using JetBrains.Threading;
using JetBrains.UI.StdApplicationUI;

namespace ReSharperPlugin.ReSharperGamificationPlugin.Achievements.UIInteractive.Wpf;

[SolutionComponent(Instantiation.DemandAnyThreadSafe)]
public class AchievementsDialogProvider(
  IShellLocks shellLocks,
  IWindowBranding branding,
  HelpSystem helpSystem) : IAchievementsDialogProvider
{
  public void ShowDialog(LifetimeDefinition ld, ISolution solution, AchievementsViewModel viewModel)
  {
    RunLater(ld.Lifetime, solution, viewModel).NoAwait();
  }

  private async Task RunLater(Lifetime lifetime, ISolution solution, AchievementsViewModel viewModel)
  {
    await Task.Delay(TimeSpan.FromMilliseconds(300), lifetime);
    await shellLocks.Dispatcher.YieldTo(lifetime);

    var window = new AchievementsDialog(viewModel, branding, helpSystem);
    solution.TryGetComponent<IMainWindow>().ShowDialogOverActiveWindow(window);
  }
}