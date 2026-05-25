using sabidos.Application.DTOs;
using sabidos.Infrastructure.Repositories;

namespace sabidos.Application.Services;

public class AchievementService
{
    private readonly AchievementRepository _repo;

    public AchievementService(
        AchievementRepository repo)
    {
        _repo = repo;
    }

    public async Task CheckAchievements(
        string userId)
    {
        var stats =
            await _repo.GetUserStats(userId);

        await CheckResumoAchievements(
            userId,
            stats);

        await CheckFlashcardAchievements(
            userId,
            stats);

        await CheckPomodoroAchievements(
            userId,
            stats);

        await CheckVeteranoAchievements(
            userId,
            stats);
    }

    private async Task CheckResumoAchievements(
        string userId,
        UserStatsResponse stats)
    {
        if (stats.ResumosCriados >= 1)
        {
            await UnlockAchievement(
                userId,
                new UserAchievementDto
                {
                    Id = "primeiro_resumo",
                    Title = "Primeiro Resumo",
                    Description =
                        "Você criou seu primeiro resumo.",
                    Goal = 1,
                    Progress =
                        stats.ResumosCriados,
                    Unlocked = true
                });
        }
    }

    private async Task CheckFlashcardAchievements(
        string userId,
        UserStatsResponse stats)
    {
        if (stats.FlashcardsCriados >= 20)
        {
            await UnlockAchievement(
                userId,
                new UserAchievementDto
                {
                    Id = "mestre_flashcards",
                    Title = "Mestre Flashcards",
                    Description =
                        "Crie 20 flashcards.",
                    Goal = 20,
                    Progress =
                        stats.FlashcardsCriados,
                    Unlocked = true
                });
        }
    }

    private async Task CheckPomodoroAchievements(
        string userId,
        UserStatsResponse stats)
    {
        if (stats.PomodorosConcluidos >= 5)
        {
            await UnlockAchievement(
                userId,
                new UserAchievementDto
                {
                    Id = "foco_inicial",
                    Title = "Foco Inicial",
                    Description =
                        "Conclua 5 pomodoros.",
                    Goal = 5,
                    Progress =
                        stats.PomodorosConcluidos,
                    Unlocked = true
                });
        }
    }

    private async Task CheckVeteranoAchievements(
        string userId,
        UserStatsResponse stats)
    {
        if (stats.TotalAcoes >= 100)
        {
            await UnlockAchievement(
                userId,
                new UserAchievementDto
                {
                    Id = "veterano",
                    Title = "Veterano",
                    Description =
                        "Realize 100 ações.",
                    Goal = 100,
                    Progress =
                        stats.TotalAcoes,
                    Unlocked = true
                });
        }
    }

    private async Task UnlockAchievement(
        string userId,
        UserAchievementDto achievement)
    {
        var exists =
            await _repo.HasAchievement(
                userId,
                achievement.Id);

        if (exists)
            return;

        await _repo.UnlockAchievement(
            userId,
            achievement);
    }
}