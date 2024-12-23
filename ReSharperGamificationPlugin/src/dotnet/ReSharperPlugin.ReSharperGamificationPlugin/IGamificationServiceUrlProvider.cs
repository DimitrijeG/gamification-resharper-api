using JetBrains.Application;

namespace ReSharperPlugin.ReSharperGamificationPlugin;

public interface IGamificationServiceUrlProvider
{
  public string ServiceUrl { get; }
}

[ShellComponent]
public class GamificationStaticServiceUrlProvider : IGamificationServiceUrlProvider
{
  public string ServiceUrl => "https://localhost:7106/";
}