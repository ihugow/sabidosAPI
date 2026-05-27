using sabidos.Application.DTOs;
using sabidos.Domain.Entities;
using sabidos.Domain.Interfaces;

namespace sabidos.Application.Services;

public class DailyMissionService
{
    private readonly IMissionsRepository _missionsRepo;
    private readonly IPointRepository _pointRepo;
    private readonly LevelService _levelService;

    private static readonly List<DailyMissionDefinition> _catalog = new()
    {
        new DailyMissionDefinition { Id = "m_resumo_1", Title = "Escritor", Description = "Crie 1 resumo hoje.", Emoji = "✍️", Goal = 1, ActionType = PointActionType.ResumoCriado, XpReward = 20 },
        new DailyMissionDefinition { Id = "m_resumo_2", Title = "Bibliotecário", Description = "Crie 2 resumos hoje.", Emoji = "📚", Goal = 2, ActionType = PointActionType.ResumoCriado, XpReward = 40 },
        new DailyMissionDefinition { Id = "m_pomodoro_1", Title = "Foco Total", Description = "Complete 3 ciclos de Pomodoro.", Emoji = "🍅", Goal = 3, ActionType = PointActionType.PomodoroCompleto, XpReward = 30 },
        new DailyMissionDefinition { Id = "m_pomodoro_2", Title = "Mestre do Tempo", Description = "Complete 5 ciclos de Pomodoro.", Emoji = "⏱️", Goal = 5, ActionType = PointActionType.PomodoroCompleto, XpReward = 50 },
        new DailyMissionDefinition { Id = "m_flashcard_1", Title = "Estudante", Description = "Responda 10 Flashcards.", Emoji = "🧠", Goal = 10, ActionType = PointActionType.FlashcardRespondido, XpReward = 25 },
        new DailyMissionDefinition { Id = "m_flashcard_2", Title = "Sabedoria Pura", Description = "Responda 20 Flashcards.", Emoji = "💡", Goal = 20, ActionType = PointActionType.FlashcardRespondido, XpReward = 50 },
        new DailyMissionDefinition { Id = "m_agenda_1", Title = "Organizado", Description = "Cadastre 2 eventos na agenda.", Emoji = "📅", Goal = 2, ActionType = PointActionType.EventoCriado, XpReward = 15 },
    };

    public DailyMissionService(
        IMissionsRepository missionsRepo, 
        IPointRepository pointRepo,
        LevelService levelService)
    {
        _missionsRepo = missionsRepo;
        _pointRepo = pointRepo;
        _levelService = levelService;
    }

    public async Task<DailyMissionsResponse> GetDailyMissions(string userId)
    {
        var today = DateTime.UtcNow.ToString("yyyy-MM-dd");
        var state = await _missionsRepo.GetUserDailyState(userId);

        // Reset Diário
        if (state == null || state.AssignedDate != today)
        {
            state = new UserDailyState
            {
                AssignedDate = today,
                RerollsUsed = 0,
                CompletedMissionIds = new List<string>(),
                ActiveMissionIds = PickRandomMissions(new List<string>())
            };
            await _missionsRepo.SaveUserDailyState(userId, state);
        }

        var missions = new List<DailyMissionDto>();
        foreach (var id in state.ActiveMissionIds)
        {
            var def = _catalog.FirstOrDefault(m => m.Id == id);
            if (def == null) continue;

            int progress = await _pointRepo.GetDailyActionCount(userId, def.ActionType.ToString(), DateTime.UtcNow);
            bool completed = progress >= def.Goal;
            bool claimed = state.CompletedMissionIds.Contains(id);

            missions.Add(new DailyMissionDto
            {
                Id = def.Id,
                Title = def.Title,
                Description = def.Description,
                Emoji = def.Emoji,
                Goal = def.Goal,
                Progress = Math.Min(progress, def.Goal),
                Completed = completed,
                Claimed = claimed,
                ActionType = def.ActionType.ToString(),
                XpReward = def.XpReward
            });
        }

        int totalXp = await _pointRepo.GetUserPoints(userId);
        int level = _levelService.CalculateLevel(totalXp);

        return new DailyMissionsResponse
        {
            Missions = missions,
            RerollsLeft = 3 - state.RerollsUsed,
            TotalXp = totalXp,
            Level = level,
            XpCurrentLevelBase = _levelService.GetCurrentLevelBaseXp(level),
            XpNextLevelThreshold = _levelService.GetNextLevelXpThreshold(level)
        };
    }

    public async Task<DailyMissionsResponse> ClaimMissionReward(string userId, string missionId)
    {
        var state = await _missionsRepo.GetUserDailyState(userId);
        if (state == null) throw new Exception("Estado diário não encontrado.");

        if (!state.ActiveMissionIds.Contains(missionId))
            throw new Exception("Esta missão não está ativa para você hoje.");

        if (state.CompletedMissionIds.Contains(missionId))
            throw new Exception("Você já coletou a recompensa desta missão.");

        var def = _catalog.FirstOrDefault(m => m.Id == missionId);
        if (def == null) throw new Exception("Definição da missão não encontrada.");

        int progress = await _pointRepo.GetDailyActionCount(userId, def.ActionType.ToString(), DateTime.UtcNow);
        if (progress < def.Goal)
            throw new Exception("Você ainda não completou o objetivo desta missão.");

        // Marca como coletada
        state.CompletedMissionIds.Add(missionId);
        await _missionsRepo.SaveUserDailyState(userId, state);

        // Dá a recompensa
        await _pointRepo.UpdatePoints(userId, def.XpReward);
        await _pointRepo.AddTransaction(userId, def.XpReward, $"Missão Diária: {def.Title}");

        return await GetDailyMissions(userId); // Agora retorna com XP atualizado
    }

    public async Task<DailyMissionsResponse> RerollMission(string userId, string missionId)
    {
        var state = await _missionsRepo.GetUserDailyState(userId);
        // if (state == null || state.RerollsUsed >= 3) 
        //     throw new Exception("Limite de rerolls atingido.");
        
        if (state == null) throw new Exception("Estado diário não encontrado.");

        if (!state.ActiveMissionIds.Contains(missionId))
            throw new Exception("Missão não encontrada.");

        state.ActiveMissionIds.Remove(missionId);
        var nextMission = PickRandomMissions(state.ActiveMissionIds, 1).First();
        state.ActiveMissionIds.Add(nextMission);
        state.RerollsUsed++;

        await _missionsRepo.SaveUserDailyState(userId, state);
        return await GetDailyMissions(userId); // Agora retorna com XP atualizado
    }

    private List<string> PickRandomMissions(List<string> excludeIds, int count = 3)
    {
        var available = _catalog.Where(m => !excludeIds.Contains(m.Id)).ToList();
        var rnd = new Random();
        return available.OrderBy(x => rnd.Next()).Take(count).Select(m => m.Id).ToList();
    }
}
