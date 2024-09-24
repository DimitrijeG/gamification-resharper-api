using ReSharperGamificationApi.Models;

namespace ReSharperGamificationApi.Services;

public interface IUserService
{
    public Task<User> FindOrSave(string uid, string firstName, string lastName);
}

public class UserService(ILogger<UserService> logger, GamificationContext context) : IUserService
{
    public async Task<User> FindOrSave(string uid, string firstName, string lastName)
    {
        logger.LogInformation("User: {uid} {firstName} {lastName}", uid, firstName, lastName);

        var newUser = new User { Uid = uid, FirstName = firstName, LastName = lastName };
        var user = await context.Users.FindOrAddAsync(context, u => u.Uid.Equals(uid), newUser);

        await context.SaveChangesAsync();

        logger.LogInformation("Saved: {id} {uid}", user.Id, user.Uid);
        return user;
    }
}