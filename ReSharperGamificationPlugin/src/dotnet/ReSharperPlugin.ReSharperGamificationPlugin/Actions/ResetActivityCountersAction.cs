using JetBrains.Application.DataContext;
using JetBrains.Application.UI.Actions;
using JetBrains.Application.UI.ActionsRevised.Menu;
using JetBrains.UsageStatistics;
using ReSharperPlugin.ReSharperGamificationPlugin.Resources;
using ReSharperPlugin.ReSharperGamificationPlugin.UsageStatistics;

namespace ReSharperPlugin.ReSharperGamificationPlugin.Actions;

[Action(typeof(Strings), nameof(Strings.ResetActivityCountersActionText))]
public class ResetActivityCountersAction : IExecutableAction
{
  public bool Update(IDataContext context, ActionPresentation presentation, DelegateUpdate nextUpdate)
  {
    return true;
  }

  public void Execute(IDataContext context, DelegateExecute nextExecute)
  {
    context.TryGetComponent<ActivityTracking>()?.ResetActivityCounters(
      UsageStatisticsActivityTracking.GroupPrefix);
  }
}

public interface IExecutableAction
{
}