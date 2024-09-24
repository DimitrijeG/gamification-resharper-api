using Microsoft.EntityFrameworkCore;
using ReSharperGamificationApi.Models;

namespace ReSharperGamificationApi.Services;

public interface IUserService
{
    public Task<int> CountAsync();
    public Task<List<User>> GetLeaderboardAsync(int pageNumber, int pageSize);
    public Task<User> FindOrSaveAsync(string uid, string firstName, string lastName);
}

public class UserService(ILogger<UserService> logger, GamificationContext context) : IUserService
{
    public Task<int> CountAsync()
    {
        return context.Users.CountAsync();
    }

    public Task<List<User>> GetLeaderboardAsync(int pageNumber, int pageSize)
    {
        return context.Users
            .OrderByDescending(u => u.Points)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
    }

    public async Task<User> FindOrSaveAsync(string uid, string firstName, string lastName)
    {
        logger.LogInformation("User: {uid} {firstName} {lastName}", uid, firstName, lastName);

        var newUser = new User { Uid = uid, FirstName = firstName, LastName = lastName };
        var user = await context.Users.FindOrAddAsync(context, u => u.Uid.Equals(uid), newUser);

        await context.SaveChangesAsync();

        logger.LogInformation("Saved: {id} {uid}", user.Id, user.Uid);
        return user;
    }
}