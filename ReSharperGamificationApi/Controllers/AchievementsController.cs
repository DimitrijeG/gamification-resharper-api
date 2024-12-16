using Asp.Versioning;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ReSharperGamificationApi.Dtos;
using ReSharperGamificationApi.Services;

namespace ReSharperGamificationApi.Controllers;

[ApiVersion(1)]
[ApiController]
[Route("api/v{v:apiVersion}/achievements")]
public class AchievementsController(
    IMapper mapper,
    IAchievementService achievementService,
    IUserService userService) : ControllerBase
{
    // GET: api/v1/achievements
    [MapToApiVersion(1)]
    [HttpGet]
    public async Task<ActionResult<IEnumerable<AchievementResponseDtoV1>>> GetAchievementsV1()
    {
        return await achievementService.Achievements
            .Select(a => mapper.Map<AchievementResponseDtoV1>(a))
            .ToListAsync();
    }

    // POST: api/v1/achievements
    [MapToApiVersion(1)]
    [HttpPost]
    [Authorize]
    public async Task<ActionResult<AchievementResponseDtoV1>> PostAchievementV1(AchievementRequestDtoV1 dto)
    {
        try
        {
            var claims = HttpContext.User;
            var uid = claims.Find(ClaimsPrincipalExtensions.UserIdClaim);
            var firstName = claims.Find(ClaimsPrincipalExtensions.FirstNameClaim);
            var lastName = claims.Find(ClaimsPrincipalExtensions.LastNameClaim);

            var user = await userService.FindOrSaveAsync(uid, firstName, lastName, dto.AccessToken);
            var saved = await achievementService.Save(user, dto.GoalId, dto.Progress);
            return CreatedAtAction(nameof(GetAchievementsV1), mapper.Map<AchievementResponseDtoV1>(saved));
        }
        catch (ClaimDoesNotExistException e)
        {
            return BadRequest(e.Message);
        }
    }
}