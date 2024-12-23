using JetBrains.Application;
using JetBrains.Application.Parts;

namespace ReSharperPlugin.ReSharperGamificationPlugin.Achievements.Autocomplete;

[ShellComponent(Instantiation.DemandAnyThreadSafe)]
public class AutocompletionProgress : SumProgress;