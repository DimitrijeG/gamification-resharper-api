namespace ReSharperGamificationApi.Models; 

public class Achievement
{
    public long Id { get; set; }
    public long GoalId { get; set; }
    public virtual Goal Goal { get; set; } = null!;
    public long UserId { get; set; }
    public virtual User User { get; set; } = null!;
    public double Progress { get; set; }
}