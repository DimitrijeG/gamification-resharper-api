using Microsoft.EntityFrameworkCore;

namespace ReSharperGamificationApi.Model;

public class GamificationContext(DbContextOptions<GamificationContext> options) : DbContext(options)
{
    public DbSet<User> Users { get; set; } = null!;
    public DbSet<Group> Groups { get; set; } = null!;
    public DbSet<Grade> Grades { get; set; } = null!;
    public DbSet<Achievement> Achievements { get; set; } = null!;
}