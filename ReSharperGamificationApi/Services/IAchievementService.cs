using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using ReSharperGamificationApi.Hubs;
using ReSharperGamificationApi.Models;
using ReSharperGamificationApi.Models.Achievements;

namespace ReSharperGamificationApi.Services;

public interface IAchievementService
{
    public DbSet<Achievement> Achievements { get; }
    public Task<ICollection<Achievement>> SaveAll(User user, string groupName, IEnumerable<string> grades);
}

public class AchievementService(
    ILogger<AchievementService> logger, 
    IHubContext<LeaderboardHub, ILeaderboardHub> hubContext,
    GamificationContext context) : IAchievementService
{
    private const double CompletedGroupPointsBonus = 100;

    public DbSet<Achievement> Achievements => context.Achievements;

    public async Task<ICollection<Achievement>> SaveAll(User user, string groupName, IEnumerable<string> gradeNames)
    {
        var group = await FindOrAddGroupAsync(groupName);
        var grades = await Task.WhenAll(gradeNames
            .Distinct()
            .Select(grade => FindOrAddGradeAsync(grade, group)));

        var unlocked = await GetByUserAndGroup(user.Id, group.Id);
        var newAchievements = grades
            .Where(grade => !unlocked.Exists(a => a.GradeId.Equals(grade.Id)))
            .Select(grade => new Achievement { Grade = grade, User = user })
            .ToList();

        if (newAchievements.Count == 0) return [];

        var groupGradesCount = await CountGradesByGroupAsync(group.Id);
        if (groupGradesCount.Equals(unlocked.Count + newAchievements.Count))
            user.Points += CompletedGroupPointsBonus;
        user.Points += newAchievements.Sum(a => a.Grade.Points);

        await context.Achievements.AddRangeAsync(newAchievements);
        await context.SaveChangesAsync();
        await hubContext.Clients.All.UpdateLeaderboard();
        return newAchievements;
    }

    private Task<List<Achievement>> GetByUserAndGroup(long userId, long groupId)
    {
        return context.Achievements
            .Where(a => a.UserId.Equals(userId) && a.Grade.GroupId.Equals(groupId))
            .ToListAsync();
    }

    private Task<int> CountGradesByGroupAsync(long groupId)
    {
        return context.Grades.CountAsync(g => g.GroupId.Equals(groupId));
    }

    private Task<Group> FindOrAddGroupAsync(string groupName)
    {
        logger.LogInformation("Group: {groupName}", groupName);
        var newGroup = new Group { Name = groupName };
        return context.Groups.FindOrAddAsync(context, g => g.Name.Equals(groupName), newGroup);
    }

    private Task<Grade> FindOrAddGradeAsync(string gradeName, Group group)
    {
        logger.LogInformation("Grade: {gradeName}", gradeName);
        var newGrade = new Grade { Name = gradeName, Points = 0, Group = group };
        return context.Grades.FindOrAddAsync(context, g => g.Name.Equals(gradeName), newGrade);
    }
}