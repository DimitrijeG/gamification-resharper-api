using System.Collections.Generic;

namespace ReSharperPlugin.ReSharperGamificationPlugin.Achievements;

public interface IAchievement
{
  string Group { get; }
  string ShortName { get; }
  IProgress Progress { get; }
  IDictionary<long, Goal> Goals { get; }
}

public readonly struct Goal(string name, int total)
{
  internal readonly string Name = name;
  internal readonly int Total = total;
}