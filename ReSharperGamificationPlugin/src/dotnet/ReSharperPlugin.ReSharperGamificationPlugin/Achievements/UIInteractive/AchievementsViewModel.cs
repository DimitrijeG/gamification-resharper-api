using System;
using System.Collections.Generic;
using System.Windows.Media;
using System.Linq;
using JetBrains.Application.UI.UIAutomation;

namespace ReSharperPlugin.ReSharperGamificationPlugin.Achievements.UIInteractive;

public class AchievementsViewModel(ICollection<AchievementItemViewModel> achievements) : AAutomation
{
  public ICollection<AchievementItemViewModel> Achievements { get; } = achievements;
}

public class AchievementItemViewModel(string title, ICollection<GradeItemViewModel> grades)
{
  public string Title { get; } = title;
  public int Unlocked => Grades.Count(grade => grade.Unlocked);
  public ICollection<GradeItemViewModel> Grades { get; } = grades;
}

public class GradeItemViewModel(string title, int total, bool unlocked, string iconTemplate, Color color)
{
  public string Title { get; } = title;
  private int Total { get; } = total;
  public bool Unlocked { get; private set; } = unlocked;
  public string IconTemplate { get; } = iconTemplate;
  public Color Color { get; } = color;
  public double Progress { get; private set; } = unlocked ? 100.0 : 0.0;

  public void SetCounted(double value)
  {
    Progress = Total != 0 ? Math.Min(100, value / Total * 100) : 0;
    Unlocked = (int)value == Total;
  }
}