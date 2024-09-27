using System.Security.Claims;
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
    private const string UserIdClaim = ClaimTypes.NameIdentifier;
    private const string FirstNameClaim = "first_name";
    private const string LastNameClaim = "last_name";

    // GET: api/v1/achievements
    [MapToApiVersion(1)]
    [HttpGet]
    public async Task<ActionResult<IEnumerable<AchievementDtoV1>>> GetAchievementsV1()
    {
        return await achievementService.Achievements
            .Select(a => mapper.Map<AchievementDtoV1>(a))
            .ToListAsync();
    }

    // POST: api/v1/achievements
    [MapToApiVersion(1)]
    [HttpPost]
    [Authorize]
    public async Task<ActionResult<IEnumerable<AchievementDtoV1>>> PostAchievementsV1(AchievementWithGradesDtoV1 dto)
    {
        try
        {
            var claims = HttpContext.User;
            var uid = claims.Find(UserIdClaim);
            var firstName = claims.Find(FirstNameClaim);
            var lastName = claims.Find(LastNameClaim);
            var user = await userService.FindOrSaveAsync(uid, firstName, lastName);
            var saved = await achievementService
                .SaveAll(user, dto.Group, dto.Grades);

            var mapped = saved.Select(mapper.Map<AchievementDtoV1>);
            return CreatedAtAction(nameof(GetAchievementsV1), mapped);
        }
        catch (ClaimDoesNotExistException e)
        {
            return BadRequest(e.Message);
        }
    }
}