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
            new Grade { Id = 1, GroupId = 1, Name = "Bronze medal", Points = 50 },
            new Grade { Id = 2, GroupId = 1, Name = "Silver medal", Points = 70 },
            new Grade { Id = 3, GroupId = 1, Name = "Gold medal", Points = 90 },
            new Grade { Id = 4, GroupId = 2, Name = "Yellow belt", Points = 40 },
            new Grade { Id = 5, GroupId = 2, Name = "Blue belt", Points = 60 },
            new Grade { Id = 6, GroupId = 2, Name = "Black belt", Points = 80 },
            new Grade { Id = 7, GroupId = 3, Name = "Amethyst ring", Points = 30 },
            new Grade { Id = 8, GroupId = 3, Name = "Emerald ring", Points = 50 },
            new Grade { Id = 9, GroupId = 3, Name = "Diamond ring", Points = 70 }
        );

        // Demo data
        modelBuilder.Entity<User>().HasData(
            new User { Id = 1, Uid = "01", FirstName = "Anna", LastName = "Johnson", Points = 0 },
            new User { Id = 2, Uid = "02", FirstName = "Brian", LastName = "Peters", Points = 310 },
            new User { Id = 3, Uid = "03", FirstName = "Catherine", LastName = "Sullivan", Points = 100 },
            new User { Id = 4, Uid = "04", FirstName = "David", LastName = "Smith", Points = 130 },
            new User { Id = 5, Uid = "05", FirstName = "Eva", LastName = "Davis", Points = 250 }
        );

        modelBuilder.Entity<Achievement>().HasData(
            new Achievement { Id = 1, GradeId = 1, UserId = 2 },
            new Achievement { Id = 2, GradeId = 2, UserId = 2 },
            new Achievement { Id = 3, GradeId = 3, UserId = 2 },
            new Achievement { Id = 4, GradeId = 4, UserId = 3 },
            new Achievement { Id = 5, GradeId = 5, UserId = 3 },
            new Achievement { Id = 6, GradeId = 7, UserId = 4 },
            new Achievement { Id = 7, GradeId = 3, UserId = 4 },
            new Achievement { Id = 8, GradeId = 6, UserId = 4 },
            new Achievement { Id = 9, GradeId = 7, UserId = 5 },
            new Achievement { Id = 10, GradeId = 8, UserId = 5 },
            new Achievement { Id = 11, GradeId = 9, UserId = 5 }
        );
    }
}