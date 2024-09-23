using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace ReSharperGamificationApi.Model;

[Index(nameof(Uid), IsUnique = true)]
public class User
{
    public long Id { get; set; }
    [MaxLength(200)] public string Uid { get; set; } = string.Empty;
    [MaxLength(200)] public string FirstName { get; set; } = string.Empty;
    [MaxLength(200)] public string LastName { get; set; } = string.Empty;
}