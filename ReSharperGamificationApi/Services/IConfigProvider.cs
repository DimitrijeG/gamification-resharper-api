using System.Text.Json;

namespace ReSharperGamificationApi.Services;

public interface IConfig
{
    int MaxUsersPerLeague { get; }
}

public class Config : IConfig
{
    public int MaxUsersPerLeague { get; }
    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNameCaseInsensitive = true
    };

    public Config(string configPath)
    {
        var jsonData = File.ReadAllText(configPath);
        var conf = JsonSerializer.Deserialize<ConfigData>(jsonData, JsonOptions);

        MaxUsersPerLeague = conf?.MaxUsersPerLeague ?? 30;
    }

    public class ConfigData
    {
        public int MaxUsersPerLeague { get; set; }
    }
}
