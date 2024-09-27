using Microsoft.EntityFrameworkCore;

namespace ReSharperGamificationApi.Models.Achievements;

[Index(nameof(GradeId), nameof(UserId), IsUnique = true)]
public class Achievement
{
    public long Id { get; set; }
    public long GradeId { get; set; }
    public virtual Grade Grade { get; set; } = null!;
    public long UserId { get; set; }
    public virtual User User { get; set; } = null!;
}