using AutoMapper;
using ReSharperGamificationApi.Dtos;

namespace ReSharperGamificationApi.Models;

public class League
{
    public long Id { get; set; }
    public long RankId { get; set; }
    public virtual Rank Rank { get; set; } = null!;
    public int UserCount { get; set; } = 0;
    public virtual ICollection<User> Users { get; set; } = [];
}

