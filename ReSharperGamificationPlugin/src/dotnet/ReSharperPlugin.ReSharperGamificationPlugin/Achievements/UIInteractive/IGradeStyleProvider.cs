using System.Windows.Media;
using JetBrains.Application;
using JetBrains.Application.ActivityTrackingNew;

namespace ReSharperPlugin.ReSharperGamificationPlugin.Achievements.UIInteractive;

public interface IGradeStyleProvider
{
  string GetIcon(string group);
  Color GetColor(string group, long goalId);
}

[ShellComponent]
public class StaticGradeStyleProvider : IGradeStyleProvider
{
  public string GetIcon(string group)
  {
    return group switch
    {
      IGamificationStatisticsTracking.Group.Refactoring => "medal",
      IGamificationStatisticsTracking.Group.Formatting => "belt",
      IGamificationStatisticsTracking.Group.Autocompletion => "ring",
      _ => default
    };
  }

  public Color GetColor(string group, long goalId)
  {
    return group switch
    {
      IGamificationStatisticsTracking.Group.Refactoring => goalId switch
      {
        1L => Color.FromRgb(206, 127, 70),
        2L => Color.FromRgb(187, 187, 187),
        3L => Color.FromRgb(232, 185, 35),
        _ => default
      },
      IGamificationStatisticsTracking.Group.Formatting => goalId switch
      {
        4L => Color.FromRgb(239, 211, 30),
        5L => Color.FromRgb(0, 111, 189),
        6L => Color.FromRgb(0, 0, 0),
        _ => default
      },
      IGamificationStatisticsTracking.Group.Autocompletion => goalId switch
      {
        7L => Color.FromRgb(153, 102, 204),
        8L => Color.FromRgb(60, 209, 110),
        9L => Color.FromRgb(103, 226, 255),
        _ => default
      },
      _ => default
    };
  }
}