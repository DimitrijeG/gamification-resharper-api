using ReSharperGamificationApi.Models;

namespace ReSharperGamificationApi.ViewModels;

public class PaginatedLeaderboardViewModel
{
    public int CurrentPage { get; set; }
    public int PageSize { get; set; }
    public int TotalPages { get; set; }
    public ICollection<LeaderboardEntry> LeaderboardEntries { get; set; } = [];
}
