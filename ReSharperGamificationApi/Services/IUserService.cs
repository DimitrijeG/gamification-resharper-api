using ReSharperGamificationApi.Models;

namespace ReSharperGamificationApi.Services;

public interface IUserService
{
    public Task<User> FindOrSaveAsync(string uid, string firstName, string lastName);
}

public class UserService(GamificationContext context) : IUserService
{
    public async Task<User> FindOrSaveAsync(string uid, string firstName, string lastName)
    {
        var newUser = new User { Uid = uid, FirstName = firstName, LastName = lastName };
        return await context.Users.FindOrAddAsync(context, u => u.Uid.Equals(uid), newUser);
    }
}