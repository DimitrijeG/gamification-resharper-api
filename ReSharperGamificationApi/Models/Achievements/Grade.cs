﻿using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace ReSharperGamificationApi.Models.Achievements;

[Index(nameof(Name), IsUnique = true)]
public class Grade
{
    public long Id { get; set; }
    [MaxLength(100)] public string Name { get; set; } = string.Empty;
    public double Points { get; set; }
    public long GroupId { get; set; }
    public virtual Group Group { get; set; } = null!;
}