using Microsoft.EntityFrameworkCore;
using ReSharperGamificationApi.Models;
using ReSharperGamificationApi.Models.Context;

namespace ReSharperGamificationApi.Services;

public interface IUserService
{
    Task<User> FindOrSaveAsync(string uid, string firstName, string lastName, string accessToken);
    Task<User?> FindByAccessTokenAsync(string accessToken);
}

public class UserService(
    GamificationContext context,
    ILeagueService leagueService) : IUserService
{
    public async Task<User> FindOrSaveAsync(string uid, string firstName, string lastName, string accessToken)
    {
        var found = await context.Users.FirstOrDefaultAsync(u => u.Uid.Equals(uid));

        if (found != null)
        {
            if (found.AccessToken == accessToken) return found;
            found.AccessToken = accessToken;
            await context.SaveChangesAsync();
            return found;
        }

        var newUser = new User { Uid = uid, FirstName = firstName, LastName = lastName, AccessToken = accessToken };
        await context.Users.AddAsync(newUser);
        await leagueService.AddUserAsync(newUser);

        await context.SaveChangesAsync();
        return newUser;
    }

    public Task<User?> FindByAccessTokenAsync(string accessToken)
    {
        return context.Users
            .Include(u => u.League)
            .FirstOrDefaultAsync(u => u.AccessToken.Equals(accessToken));
    }
}