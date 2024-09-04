using ReSharperGamificationApi.Models;

namespace ReSharperGamificationApi.Services
{
    public interface IAchievementService
    {
        public Task<IEnumerable<Achievement>> SaveAll(string group, IEnumerable<string> grades, string user);
    }
}
