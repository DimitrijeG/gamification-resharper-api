using AutoMapper;
using Microsoft.AspNetCore.SignalR;
using ReSharperGamificationApi.Models;
using ReSharperGamificationApi.Services;

namespace ReSharperGamificationApi.Hubs;

public interface ILeagueHub
{
    public Task UpdateLeague();
}

public class LeagueHub(IMapper mapper, ILeagueService service) : Hub<ILeagueHub>
{
    // ReSharper disable once UnusedMember.Global
    public async Task<IEnumerable<LeaderboardEntry>> GetUpdatedLeague(long leagueId)
    {
        return service.GetLeaderboard(await service.FindAsync(leagueId));
    }
}