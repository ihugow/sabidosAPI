using sabidos.Application.DTOs;
using sabidos.Application.Interfaces;
using sabidos.Domain.Interfaces;

namespace sabidos.Application.Services;

public class AchievementService
{
    private readonly IAchievementRepository _repo;
    private readonly IPointRepository _pointRepo;

    public AchievementService(
        IAchievementRepository repo,
        IPointRepository pointRepo)
    {
        _repo = repo;
        _pointRepo = pointRepo;
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

        await CheckAgendaAchievements(
            userId,
            stats);

        await CheckVeteranoAchievements(
            userId,
            stats);
    }

    private async Task CheckAgendaAchievements(
        string userId,
        UserStatsResponse stats)
    {
        if (stats.EventosCriados >= 10)
        {
            await UnlockAchievement(
                userId,
                new UserAchievementDto
                {
                    Id = "organizado",
                    Title = "Organizado",
                    Description =
                        "Cadastre 10 eventos na agenda.",
                    Goal = 10,
                    Progress =
                        stats.EventosCriados,
                    Unlocked = true,
                    XpReward = 150
                });
        }
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
                    Unlocked = true,
                    XpReward = 50
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
                    Unlocked = true,
                    XpReward = 200
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
                    Unlocked = true,
                    XpReward = 100
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
                    Unlocked = true,
                    XpReward = 500
                });
        }
    }

    public async Task<List<string>> GetUserAchievementsIds(string userId)
    {
        var achievements = await _repo.GetUserAchievements(userId);
        return achievements.Select(a => a.Id).ToList();
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

        // 1. Salva a conquista como desbloqueada
        await _repo.UnlockAchievement(
            userId,
            achievement);

        // 2. RECOMPENSA O USUÁRIO COM XP
        if (achievement.XpReward > 0)
        {
            await _pointRepo.UpdatePoints(userId, achievement.XpReward);

            await _pointRepo.AddTransaction(
                userId, 
                achievement.XpReward, 
                $"Conquista: {achievement.Title}"
            );
        }
    }
}