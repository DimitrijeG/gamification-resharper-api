using JetBrains.Application;
using JetBrains.Application.Parts;

namespace ReSharperPlugin.ReSharperGamificationPlugin.Achievements.Formatting;

[ShellComponent(Instantiation.DemandAnyThreadSafe)]
public class FormattingProgress : SumProgress;