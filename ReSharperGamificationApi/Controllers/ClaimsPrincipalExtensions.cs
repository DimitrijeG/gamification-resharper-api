using System.Security.Claims;

namespace ReSharperGamificationApi.Controllers;

public static class ClaimsPrincipalExtensions
{
    public const string UserIdClaim = ClaimTypes.NameIdentifier;
    public const string FirstNameClaim = "first_name";
    public const string LastNameClaim = "last_name";

    public static string Find(this ClaimsPrincipal claims, string type)
    {
        var claim = claims.FindFirst(type)?.Value;
        return claim ?? throw new ClaimDoesNotExistException(type);
    }
}

public class ClaimDoesNotExistException(string claim)
    : Exception($"Claim {claim} does not exist");