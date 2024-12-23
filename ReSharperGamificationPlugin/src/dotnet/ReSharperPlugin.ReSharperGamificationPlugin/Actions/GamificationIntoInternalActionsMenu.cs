using JetBrains.Application.UI.Actions.InternalMenu;
using JetBrains.Application.UI.ActionSystem.ActionsRevised.Menu;
using ReSharperPlugin.ReSharperGamificationPlugin.Resources;

namespace ReSharperPlugin.ReSharperGamificationPlugin.Actions;

[ActionGroup(
  ActionGroupInsertStyles.Submenu,
  TextResourceType = typeof(Strings),
  TextResourceName = nameof(Strings.GamificationActionGroupText))]
public class GamificationIntoInternalActionsMenu : IAction, IInsertLast<InternalActionsMenu>
{
  public GamificationIntoInternalActionsMenu(
    ClearAchievementsAction clearAchievementsAction,
    ResetActivityCountersAction resetActivityCountersAction)
  {
  }
}