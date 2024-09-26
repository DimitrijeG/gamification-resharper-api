using AutoMapper;
using Microsoft.AspNetCore.SignalR;
using ReSharperGamificationApi.Dtos;
using ReSharperGamificationApi.Services;

namespace ReSharperGamificationApi.Hubs;

public interface ILeaderboardHub
{
    public Task UpdateLeaderboard();
}

public class LeaderboardHub(IMapper mapper, ILeaderboardService service) : Hub<ILeaderboardHub>
{
    // ReSharper disable once UnusedMember.Global
    public async Task<IEnumerable<LeaderboardEntryDtoV1>> GetUpdatedLeaderboard(int pageNumber, int pageSize)
    {
        return (await service.GetPaginatedAsync(pageNumber, pageSize))
            .Select(mapper.Map<LeaderboardEntryDtoV1>);
    }
}