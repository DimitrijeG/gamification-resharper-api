using Microsoft.EntityFrameworkCore;

namespace ReSharperGamificationApi.Models
{
    public class AchievementContext(DbContextOptions<AchievementContext> options) 
        : DbContext(options)
    {
        public DbSet<Achievement> Achievements { get; set; } = null!;
    }
}
