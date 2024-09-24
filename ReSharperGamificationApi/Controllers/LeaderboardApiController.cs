using Asp.Versioning;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using ReSharperGamificationApi.Dtos;
using ReSharperGamificationApi.Services;

namespace ReSharperGamificationApi.Controllers;

[Route("api/v{v:apiVersion}/leaderboard")]
[ApiController]
[ApiVersion(1)]
public class LeaderboardApiController(
    IMapper mapper,
    IUserService userService) : ControllerBase
{
    // GET: api/v1/leaderboard
    [MapToApiVersion(1)]
    [HttpGet]
    public async Task<ActionResult<IEnumerable<LeaderboardEntryDtoV1>>> GetLeaderboardV1(int pageNumber = 1,
        int pageSize = 10)
    {
        var users = await userService.GetLeaderboardAsync(pageNumber, pageSize);

        var startingPosition = (pageNumber - 1) * pageSize + 1;
        var mapped = users
            .Select((user, index) =>
            {
                var dto = mapper.Map<LeaderboardEntryDtoV1>(user);
                dto.Position = startingPosition + index;
                return dto;
            });
        return Ok(mapped);
    }
}