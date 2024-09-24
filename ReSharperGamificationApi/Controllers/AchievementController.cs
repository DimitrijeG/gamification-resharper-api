using System.Security.Claims;
using Asp.Versioning;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ReSharperGamificationApi.Dtos;
using ReSharperGamificationApi.Services;

namespace ReSharperGamificationApi.Controllers;

[ApiVersion(1)]
[ApiController]
[Route("api/v{v:apiVersion}/achievements")]
public class AchievementController(
    ILogger<AchievementController> logger,
    IMapper mapper,
    IAchievementService achievementService,
    IUserService userService) : ControllerBase
{
    private const string UserIdClaim = ClaimTypes.NameIdentifier;
    private const string FirstNameClaim = "first_name";
    private const string LastNameClaim = "last_name";

    // POST: api/v1/achievements
    [MapToApiVersion(1)]
    [HttpPost]
    [Authorize]
    public async Task<ActionResult<IEnumerable<AchievementDtoV1>>> PostAchievementV1(AchievementWithGradesDtoV1 dto)
    {
        try
        {
            var claims = HttpContext.User;
            var uid = claims.Find(UserIdClaim);
            var firstName = claims.Find(FirstNameClaim);
            var lastName = claims.Find(LastNameClaim);
            logger.LogInformation("User claims {firstName} {lastName}", firstName, lastName);

            var user = await userService.FindOrSave(uid, firstName, lastName);
            var saved = await achievementService
                .SaveAll(user, dto.Group, dto.Grades);

            return Ok(saved.Select(mapper.Map<AchievementDtoV1>));
        }
        catch (ClaimDoesNotExistException e)
        {
            return BadRequest(e.Message);
        }
    }
}