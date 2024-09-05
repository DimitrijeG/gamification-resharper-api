using System.ComponentModel.DataAnnotations;

namespace ReSharperGamificationApi.Models;

public class Achievement
{
    public long Id { get; set; }
    [MaxLength(100)]
    public string UserId { get; set; } = string.Empty;
    [MaxLength(200)]
    public string Group { get; set; } = string.Empty;
    [MaxLength(200)]
    public string Grade { get; set; } = string.Empty;
}
