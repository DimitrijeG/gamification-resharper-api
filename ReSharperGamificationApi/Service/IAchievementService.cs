using Microsoft.EntityFrameworkCore;
using ReSharperGamificationApi.Model;

namespace ReSharperGamificationApi.Service;

public interface IAchievementService
{
    public Task<IEnumerable<Achievement>> SaveAll(User user, string groupName, IEnumerable<string> grades);
}

public class AchievementService(ILogger<AchievementService> logger, GamificationContext context) : IAchievementService
{
    public async Task<IEnumerable<Achievement>> SaveAll(User user, string groupName, IEnumerable<string> grades)
    {
        logger.LogInformation("Group: {groupName}", groupName);

        var group = await FindOrAddGroupAsync(groupName);
        var achievements = new List<Achievement>();

        foreach (var gradeName in grades.Distinct())
        {
            logger.LogInformation("Grade: {gradeName}", gradeName);

            var grade = await FindOrAddGradeAsync(gradeName, group);
            if (await AchievementExists(user.Id, grade.Id)) continue;

            achievements.Add(new Achievement { Grade = grade, User = user });
        }

        await context.Achievements.AddRangeAsync(achievements);
        await context.SaveChangesAsync();

        return achievements;
    }

    private Task<bool> AchievementExists(long userId, long gradeId)
    {
        return context.Achievements
            .AnyAsync(a => a.UserId.Equals(userId) && a.GradeId.Equals(gradeId));
    }

    private Task<Group> FindOrAddGroupAsync(string groupName)
    {
        var newGroup = new Group { Name = groupName };
        return context.Groups.FindOrAddAsync(context, g => g.Name.Equals(groupName), newGroup);
    }

    private Task<Grade> FindOrAddGradeAsync(string gradeName, Group group)
    {
        var newGrade = new Grade { Name = gradeName, Points = 0, Group = group };
        return context.Grades.FindOrAddAsync(context, g => g.Name.Equals(gradeName), newGrade);
    }
}