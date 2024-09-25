using Microsoft.EntityFrameworkCore;
using ReSharperGamificationApi.Models;

namespace ReSharperGamificationApi.Services;

public interface ILeaderboardService
{
    public Task<int> CountAsync();
    public Task<ICollection<LeaderboardEntry>> GetPaginatedAsync(int pageNumber, int pageSize);
}

public class LeaderboardService(GamificationContext context) : ILeaderboardService
{
    public Task<int> CountAsync() => context.Users.CountAsync();

    public async Task<ICollection<LeaderboardEntry>> GetPaginatedAsync(int pageNumber, int pageSize)
    {
        var startingPosition = (pageNumber - 1) * pageSize + 1;

        var paginatedUsers = await context.Users
            .OrderByDescending(u => u.Points)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return paginatedUsers
            .Select((user, index) => new LeaderboardEntry
            {
                Position = startingPosition + index,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Points = user.Points
            }).ToList();
    }
}