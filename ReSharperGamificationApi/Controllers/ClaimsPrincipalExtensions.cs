using System.Security.Claims;

namespace ReSharperGamificationApi.Controllers;

public static class ClaimsPrincipalExtensions
{
    public static string Find(this ClaimsPrincipal claims, string type)
    {
        var claim = claims.FindFirst(type)?.Value;
        if (claim == null)
            throw new ClaimDoesNotExistException(type);

        return claim;
    }
}

public class ClaimDoesNotExistException(string claim)
    : Exception($"Claim {claim} does not exist");