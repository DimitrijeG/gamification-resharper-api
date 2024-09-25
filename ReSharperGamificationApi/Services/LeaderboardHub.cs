using Microsoft.AspNetCore.SignalR;

namespace ReSharperGamificationApi.Services;

public class LeaderboardHub : Hub;

public static class LeaderboardHubExtensions
{
    public static async Task UpdateLeaderboardAsync(this IHubContext<LeaderboardHub> hubContext)
    {
        await hubContext.Clients.All.SendAsync("UpdateLeaderboard");
    }
}