using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace ReSharperGamificationApi.Models;

[Index(nameof(Name), IsUnique = true)]
public class Grade
{
    public long Id { get; set; }
    [MaxLength(100)] public string Name { get; set; } = string.Empty;
    public double Points { get; set; }
    public long GroupId { get; set; }
    public Group Group { get; set; } = null!;
}