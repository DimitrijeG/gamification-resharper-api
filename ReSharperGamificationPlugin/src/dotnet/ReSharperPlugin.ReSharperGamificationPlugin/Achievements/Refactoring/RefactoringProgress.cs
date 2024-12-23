using JetBrains.Application;
using JetBrains.Application.Parts;

namespace ReSharperPlugin.ReSharperGamificationPlugin.Achievements.Refactoring;

[ShellComponent(Instantiation.DemandAnyThreadSafe)]
public class RefactoringProgress : UsedPercentageProgress
{
  protected override int TotalCount => 11;
}