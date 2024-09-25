using Microsoft.AspNetCore.Mvc;
using ReSharperGamificationApi.Services;
using ReSharperGamificationApi.ViewModels;

namespace ReSharperGamificationApi.Controllers;

[Route("leaderboard")]
[Controller]
public class LeaderboardController(ILeaderboardService leaderboardService) : Controller
{
    // GET: leaderboard
    [HttpGet]
    public async Task<IActionResult> Leaderboard(int pageNumber = 1, int pageSize = 10)
    {
        var totalUsers = await leaderboardService.CountAsync();
        var totalPages = (int)Math.Ceiling(totalUsers / (double)pageSize);

        var entries = await leaderboardService
            .GetPaginatedAsync(pageNumber, pageSize);

        var viewModel = new PaginatedLeaderboardViewModel
        {
            CurrentPage = pageNumber,
            PageSize = pageSize,
            TotalPages = totalPages,
            LeaderboardEntries = entries
        };

        return View(viewModel);
    }
}