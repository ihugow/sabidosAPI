using sabidos.Application.DTOs;
using sabidos.Infrastructure.Repositories;

namespace sabidos.Application.Services;

public class GamificationService
{
    private readonly AchievementRepository _repo;

    private readonly LevelService _levelService;

    public GamificationService(
        AchievementRepository repo,
        LevelService levelService)
    {
        _repo = repo;
        _levelService = levelService;
    }

    public async Task<UserGamificationProfile>
        GetProfile(string userId)
    {
        var stats =
            await _repo.GetUserStats(userId);

        int level =
            _levelService.CalculateLevel(
                stats.TotalXp);

        var achievements =
            await _repo.GetUserAchievements(
                userId);

        return new UserGamificationProfile
        {
            Stats = stats,

            Achievements = achievements,

            TotalXp = stats.TotalXp,

            Level = level
        };
    }
}