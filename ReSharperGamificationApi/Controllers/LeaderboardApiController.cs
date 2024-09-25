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
    ILeaderboardService leaderboardService) : ControllerBase
{
    // GET: api/v1/leaderboard
    [MapToApiVersion(1)]
    [HttpGet]
    public async Task<ActionResult<IEnumerable<LeaderboardEntryDtoV1>>> GetLeaderboardV1(
        int pageNumber = 1, int pageSize = 10)
    {
        var entries = await leaderboardService
            .GetPaginatedAsync(pageNumber, pageSize);

        return Ok(entries.Select(mapper.Map<LeaderboardEntryDtoV1>));
    }
}