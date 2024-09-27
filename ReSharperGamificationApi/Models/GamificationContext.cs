using Microsoft.EntityFrameworkCore;
using ReSharperGamificationApi.Models.Achievements;
using System.Text.Json;

namespace ReSharperGamificationApi.Models;

public class GamificationContext(DbContextOptions<GamificationContext> options) : DbContext(options)
{
    public DbSet<User> Users { get; set; } = null!;
    public DbSet<Group> Groups { get; set; } = null!;
    public DbSet<Grade> Grades { get; set; } = null!;
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

        context.Users.AddRange(seedData.Users);
        context.Groups.AddRange(seedData.Groups);
        context.Grades.AddRange(seedData.Grades);
        context.Achievements.AddRange(seedData.Achievements);
        context.SaveChanges();
    }

    public class SeedData
    {
        public List<Group> Groups { get; set; } = [];
        public List<Grade> Grades { get; set; } = [];
        public List<User> Users { get; set; } = [];
        public List<Achievement> Achievements { get; set; } = [];
    }
}