using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace ReSharperGamificationApi.Models.Context;

public class GamificationContext(DbContextOptions<GamificationContext> options) : DbContext(options)
{
    public DbSet<Rank> Ranks { get; set; } = null!;
    public DbSet<League> Leagues { get; set; } = null!;
    public DbSet<User> Users { get; set; } = null!;
    public DbSet<Group> Groups { get; set; } = null!;
    public DbSet<Goal> Goals { get; set; } = null!;
    public DbSet<Achievement> Achievements { get; set; } = null!;
}

public static class DatabaseSeedingExtensions
{
    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNameCaseInsensitive = true
    };

    public static void SeedDatabase(this GamificationContext context, string file)
    {
        if (context.Users.Any()) return;

        var path = Path.Combine(AppContext.BaseDirectory, "Data", "SeedData", file);
        var jsonData = File.ReadAllText(path);
        var seedData = JsonSerializer.Deserialize<SeedData>(jsonData, JsonOptions);
        if (seedData == null) return;

        context.Groups.AddRange(seedData.Groups);
        context.Goals.AddRange(seedData.Goals);
        context.Ranks.AddRange(seedData.Ranks);
        context.Leagues.AddRange(seedData.Leagues);
        context.Users.AddRange(seedData.Users);
        context.Achievements.AddRange(seedData.Achievements);
        context.SaveChanges();
    }

    public class SeedData
    {
        public List<Group> Groups { get; set; } = [];
        public List<Goal> Goals { get; set; } = [];
        public List<Rank> Ranks { get; set; } = [];
        public List<League> Leagues { get; set; } = [];
        public List<User> Users { get; set; } = [];
        public List<Achievement> Achievements { get; set; } = [];
    }
}