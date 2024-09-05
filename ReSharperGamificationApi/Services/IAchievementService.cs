using ReSharperGamificationApi.Models;

namespace ReSharperGamificationApi.Services
{
    public interface IAchievementService
    {
        public Task<IEnumerable<Achievement>> SaveAll(string userId, string group, IEnumerable<string> grades);
    }
}
