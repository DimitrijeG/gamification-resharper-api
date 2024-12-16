using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using ReSharperGamificationApi.Hubs;
using ReSharperGamificationApi.Models;
using ReSharperGamificationApi.Models.Context;

namespace ReSharperGamificationApi.Services;

public interface IAchievementService
{
    public DbSet<Achievement> Achievements { get; }
    public Task<Achievement> Save(User user, long goalId, double progress);
}

public class AchievementService(
    GamificationContext context,
    IHubContext<LeagueHub, ILeagueHub> hubContext) : IAchievementService
{
    public DbSet<Achievement> Achievements => context.Achievements;

    public async Task<Achievement> Save(User user, long goalId, double progress)
    {
        var achievement = await context.Achievements.FirstOrDefaultAsync(
                              a => a.UserId.Equals(user.Id) && a.GoalId.Equals(goalId)) ??
                          (await context.Achievements.AddAsync(
                              new Achievement { UserId = user.Id, GoalId = goalId, Progress = progress })).Entity;

        achievement.Progress = progress;
        if (achievement.Progress.Equals(1))
        {
            user.Points += (await context.Goals.FindAsync(goalId))!.Points;
        }

        await context.SaveChangesAsync();
        await hubContext.Clients.All.UpdateLeague();
        return achievement;
    }
}