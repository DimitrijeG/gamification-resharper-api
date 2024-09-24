using Microsoft.EntityFrameworkCore;

namespace ReSharperGamificationApi.Models;

[Index(nameof(GradeId), nameof(UserId), IsUnique = true)]
public class Achievement
{
    public long Id { get; set; }
    public long GradeId { get; set; }
    public Grade Grade { get; set; } = null!;
    public long UserId { get; set; }
    public User User { get; set; } = null!;
}