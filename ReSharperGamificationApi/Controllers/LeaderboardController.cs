using Microsoft.AspNetCore.Mvc;

namespace ReSharperGamificationApi.Controllers;

[Route("leaderboard")]
[Controller]
public class LeaderboardController : Controller
{
    // GET: leaderboard
    [HttpGet]
    public IActionResult Index()
    {
        return View();
    }
}