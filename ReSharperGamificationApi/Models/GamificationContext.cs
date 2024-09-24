using Microsoft.EntityFrameworkCore;
using ReSharperGamificationApi.Models.Achievements;

namespace ReSharperGamificationApi.Models;

public class GamificationContext(DbContextOptions<GamificationContext> options) : DbContext(options)
{
    public DbSet<User> Users { get; set; } = null!;
    public DbSet<Group> Groups { get; set; } = null!;
    public DbSet<Grade> Grades { get; set; } = null!;
    public DbSet<Achievement> Achievements { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Group>().HasData(
            new Group { Id = 1, Name = "Refactoring" },
            new Group { Id = 2, Name = "Formatting" },
            new Group { Id = 3, Name = "Autocompletion" }
        );

        modelBuilder.Entity<Grade>().HasData(
            new Grade { Id = 1, GroupId = 1, Name = "Bronze medal" },
            new Grade { Id = 2, GroupId = 1, Name = "Silver medal" },
            new Grade { Id = 3, GroupId = 1, Name = "Gold medal" },
            new Grade { Id = 4, GroupId = 2, Name = "Yellow belt" },
            new Grade { Id = 5, GroupId = 2, Name = "Blue belt" },
            new Grade { Id = 6, GroupId = 2, Name = "Black belt" },
            new Grade { Id = 7, GroupId = 3, Name = "Amethyst ring" },
            new Grade { Id = 8, GroupId = 3, Name = "Emerald ring" },
            new Grade { Id = 9, GroupId = 3, Name = "Diamond ring" }
        );
    }
}