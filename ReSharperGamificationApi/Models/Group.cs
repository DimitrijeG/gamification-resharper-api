using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace ReSharperGamificationApi.Models;

[Index(nameof(Name), IsUnique = true)]
public class Group
{
    public long Id { get; set; }
    [MaxLength(200)] public string Name { get; set; } = string.Empty;
}