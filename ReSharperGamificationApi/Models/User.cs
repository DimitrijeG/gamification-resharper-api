using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace ReSharperGamificationApi.Models;

[Index(nameof(Uid), IsUnique = true)]
public class User
{
    public long Id { get; set; }
    [MaxLength(200)] public string Uid { get; set; } = string.Empty;
    [MaxLength(200)] public string FirstName { get; set; } = string.Empty;
    [MaxLength(200)] public string LastName { get; set; } = string.Empty;
    public long LeagueId { get; set; }
    public virtual League League { get; set; } = null!;
    public double Points { get; set; }
    [MaxLength(400)] public string AccessToken { get; set; } = string.Empty;
}

