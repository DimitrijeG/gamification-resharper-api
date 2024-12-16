using System.ComponentModel.DataAnnotations;

namespace ReSharperGamificationApi.Models;

public class Goal
{
    public long Id { get; set; }
    [MaxLength(200)] public string Name { get; set; } = string.Empty;
    [MaxLength(400)] public string Description { get; set; } = string.Empty;
    public GoalLifetime Lifetime { get; set; }
    public double Points { get; set; }
    public long GroupId { get; set; }
    public virtual Group Group { get; set; } = null!;
}

public enum GoalLifetime
{
    Permanent,
    Weekly,
    Daily
}

