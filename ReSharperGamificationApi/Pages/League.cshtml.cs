using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ReSharperGamificationApi.Controllers;
using ReSharperGamificationApi.Models;
using ReSharperGamificationApi.Services;

namespace ReSharperGamificationApi.Pages;

public class LeagueModel(
    IUserService userService,
    ILeagueService leagueService) : PageModel
{
    public long LeagueId { get; set; }
    public string Rank { get; set; } = string.Empty;
    public ICollection<LeaderboardEntry> LeaderboardEntries { get; set; } = [];
    [FromRoute] public string AccessToken { get; set; } = string.Empty;

    public async Task OnGetAsync()
    {
        var user = await userService.FindByAccessTokenAsync(AccessToken);
        if (user == null) return;

        LeagueId = user.League.Id;
        Rank = user.League.Rank.Name;
        LeaderboardEntries = leagueService.GetLeaderboard(user.League).ToList();
    }
}