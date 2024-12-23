using JetBrains.Application.DataContext;
using JetBrains.Application.UI.Actions;
using JetBrains.Application.UI.ActionsRevised.Menu;
using ReSharperPlugin.ReSharperGamificationPlugin.Achievements.Storage;
using ReSharperPlugin.ReSharperGamificationPlugin.Resources;

namespace ReSharperPlugin.ReSharperGamificationPlugin.Actions;

[Action(typeof(Strings), nameof(Strings))]
public class ClearAchievementsAction : IExecutableAction
{
  public bool Update(IDataContext context, ActionPresentation presentation, DelegateUpdate nextUpdate)
  {
    return true;
  }

  public void Execute(IDataContext context, DelegateExecute nextExecute)
  {
    context.TryGetComponent<IAchievementDbManager>()?.WithDb(db => db.Clear());
  }
}