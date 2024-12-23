using JetBrains.Application.BuildScript.Application;
using JetBrains.Application.BuildScript.Application.Zones;
using JetBrains.Application.UI.Automation;
using JetBrains.Application.UI.Help;

namespace ReSharperPlugin.ReSharperGamificationPlugin.Achievements.UIInteractive.Wpf;

[View]
[ZoneMarker(typeof(IWpfUIEnvZone))]
public partial class AchievementsDialog : IView<AchievementsViewModel>
{
  public AchievementsDialog(AchievementsViewModel viewModel, IWindowBranding branding, HelpSystem helpSystem)
  {
    InitializeComponent();
    DataContext = viewModel;
  }
}