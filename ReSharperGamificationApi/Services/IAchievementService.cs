using Microsoft.EntityFrameworkCore;
using ReSharperGamificationApi.Models;
using ReSharperGamificationApi.Models.Achievements;

namespace ReSharperGamificationApi.Services;

public interface IAchievementService
{
    public DbSet<Achievement> Achievements { get; }
    public Task<ICollection<Achievement>> GetAll();
    public Task<ICollection<Achievement>> SaveAll(User user, string groupName, IEnumerable<string> grades);
}

public class AchievementService(ILogger<AchievementService> logger, GamificationContext context) : IAchievementService
{
    private const double CompletedGroupPointsBonus = 100;

    public DbSet<Achievement> Achievements => context.Achievements;

    public async Task<ICollection<Achievement>> GetAll()
    {
        return await context.Achievements.ToListAsync();
    }

    public async Task<ICollection<Achievement>> SaveAll(User user, string groupName, IEnumerable<string> gradeNames)
    {
        var group = await FindOrAddGroupAsync(groupName);

        var grades = await Task.WhenAll(gradeNames
            .Distinct()
            .Select(grade => FindOrAddGradeAsync(grade, group)));

        var achievements = await GetByUser(user.Id);
        var newAchievements = grades
            .Where(grade => !achievements.Exists(a => a.Grade.Equals(grade)))
            .Select(grade => new Achievement { Grade = grade, User = user })
            .ToList();

        user.Points += grades.Sum(g => g.Points);
        if (group.Grades.Count.Equals(achievements.Count + newAchievements.Count))
            user.Points += CompletedGroupPointsBonus;

        await context.Achievements.AddRangeAsync(achievements);
        await context.SaveChangesAsync();

        return newAchievements;
    }

    private Task<List<Achievement>> GetByUser(long userId)
    {
        return context.Achievements
            .Where(a => a.User.Id.Equals(userId))
            .ToListAsync();
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