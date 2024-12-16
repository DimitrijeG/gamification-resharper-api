using AutoMapper;
using Microsoft.EntityFrameworkCore;
using ReSharperGamificationApi.Models;
using ReSharperGamificationApi.Models.Context;

namespace ReSharperGamificationApi.Services;

public interface ILeagueService
{
    public Task AddUserAsync(User user, long rankId = -1);
    public Task<League> FindAsync(long leagueId);
    public IEnumerable<LeaderboardEntry> GetLeaderboard(League league);
}

public class LeagueService : ILeagueService
{
    private GamificationContext Context { get; }
    private IMapper Mapper { get; }
    private IConfig Config { get; }
    private Dictionary<long, League> LastLeague { get; } = [];
    private Mutex Lock { get; } = new();

    public LeagueService(GamificationContext context, IMapper mapper, IConfig config)
    {
        Context = context;
        Mapper = mapper;
        Config = config;

        foreach (var rank in context.Ranks.Select(r => r.Id))
        {
            var leagues = context.Leagues
                .Where(l => l.RankId == rank && l.UserCount != Config.MaxUsersPerLeague)
                .OrderBy(l => l.UserCount);

            LastLeague[rank] = !leagues.Any()
                ? Context.Leagues.Add(new League { RankId = rank}).Entity
                : leagues.First();
        }
    }

    public async Task AddUserAsync(User user, long rankId = -1)
    {
        if (rankId == -1)
            rankId = await Context.Ranks.MinAsync(r => r.Id); // the lowest rank

        lock (Lock)
        {
            var league = LastLeague[rankId];
            user.League = league;
            ++league.UserCount;

            if (league.UserCount >= Config.MaxUsersPerLeague)
                LastLeague[rankId] = GetNextLeague(rankId);
        }
    }

    private League GetNextLeague(long rankId)
    {
        return Context.Leagues.FirstOrDefault(l => l.RankId == rankId && l.UserCount == 0)
               ?? Context.Leagues.Add(new League { RankId = rankId}).Entity;
    }

    public async Task<League> FindAsync(long leagueId)
    {
        return await Context.Leagues.FindAsync(leagueId)
            ?? throw new InvalidOperationException("Missing league.");
    }

    public IEnumerable<LeaderboardEntry> GetLeaderboard(League league)
    {
        return league.Users.OrderByDescending(u => u.Points).Select((u, i) =>
        {
            var entry = Mapper.Map<LeaderboardEntry>(u);
            entry.Position = i + 1;
            return entry;
        });
    }
}