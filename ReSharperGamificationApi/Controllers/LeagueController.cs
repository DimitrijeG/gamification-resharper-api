using Asp.Versioning;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using ReSharperGamificationApi.Models;
using ReSharperGamificationApi.Services;

namespace ReSharperGamificationApi.Controllers;

[Route("api/v{v:apiVersion}/league")]
[ApiController]
[ApiVersion(1)]
public class LeagueController(
    IUserService userService,
    ILeagueService leagueService) : ControllerBase
{
    // GET: api/v1/league
    [MapToApiVersion(1)]
    [HttpGet("{accessToken}")]
    public async Task<ActionResult<IEnumerable<LeaderboardEntry>>> GetLeagueV1(string accessToken)
    {
        try
        {
            var user = await userService.FindByAccessTokenAsync(accessToken);
            return Ok(leagueService.GetLeaderboard(user.League));
        }
        catch (ClaimDoesNotExistException e)
        {
            return BadRequest(e.Message);
        }
    }
}