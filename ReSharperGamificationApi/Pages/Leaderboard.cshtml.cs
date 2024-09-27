using Microsoft.AspNetCore.Mvc.RazorPages;
using ReSharperGamificationApi.Models;
using ReSharperGamificationApi.Services;

namespace ReSharperGamificationApi.Pages;

public class LeaderboardModel(ILeaderboardService service) : PageModel
{
    public int TotalPages { get; set; }
    public int CurrentPage { get; set; }
    public int PageSize { get; set; }
    public ICollection<LeaderboardEntry> LeaderboardEntries { get; set; } = new List<LeaderboardEntry>();

    public int PreviousPage => CurrentPage - 1;
    public int NextPage => CurrentPage + 1;
    public bool IsFirstPage => CurrentPage == 1;
    public bool IsLastPage => CurrentPage == TotalPages;

    public async Task OnGetAsync(int pageNumber = 1, int pageSize = 10)
    {
        var totalUsers = await service.CountAsync();
        TotalPages = (int)Math.Ceiling(totalUsers / (double)pageSize);
        CurrentPage = pageNumber;
        PageSize = pageSize;
        LeaderboardEntries = await service.GetPaginatedAsync(pageNumber, pageSize);
    }
}