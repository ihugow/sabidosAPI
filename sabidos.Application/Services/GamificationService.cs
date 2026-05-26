using sabidos.Application.DTOs;
using sabidos.Application.Interfaces;

namespace sabidos.Application.Services;

public class GamificationService
{
    private readonly IAchievementRepository _repo;

    private readonly LevelService _levelService;

    public GamificationService(
        IAchievementRepository repo,
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