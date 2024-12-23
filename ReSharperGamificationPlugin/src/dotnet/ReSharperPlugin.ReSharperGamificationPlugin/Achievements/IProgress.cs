using System.Collections.Generic;
using System.Linq;

namespace ReSharperPlugin.ReSharperGamificationPlugin.Achievements;

public interface IProgress
{
  double Calculate(IEnumerable<int> counters);
}

public abstract class SumProgress : IProgress
{
  public double Calculate(IEnumerable<int> counters)
  {
    return counters.Sum();
  }
}

public abstract class UsedPercentageProgress : IProgress
{
  protected abstract int TotalCount { get; }

  public double Calculate(IEnumerable<int> counters)
  {
    var counted = counters.Count(counter => counter > 0);
    return (double)counted / TotalCount * 100;
  }
}