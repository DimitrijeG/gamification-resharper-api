﻿using ReSharperGamificationApi.Models;

namespace ReSharperGamificationApi.Services
{
    public class AchievementService(AchievementContext context, ILogger<AchievementService> logger) : IAchievementService
    {
        public async Task<IEnumerable<Achievement>> SaveAll(string userId, string group, IEnumerable<string> grades)
        {
            var achievements = grades
                .Select(grade => new Achievement { UserId = userId, Group = group, Grade = grade })
                .ToList();

            foreach (var achievement in achievements)
            {
                context.Achievements.Add(achievement);
                logger.LogInformation("Saving grade: {Grade}", achievement.Grade);
            }

            await context.SaveChangesAsync();

            return achievements;
        }
    }
}
