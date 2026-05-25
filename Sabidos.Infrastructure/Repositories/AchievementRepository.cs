using Google.Cloud.Firestore;
using sabidos.Application.DTOs;

namespace sabidos.Infrastructure.Repositories;

public class AchievementRepository
{
    private readonly FirestoreDb _db;

    public AchievementRepository(FirestoreDb db)
    {
        _db = db;
    }

    private DocumentReference GetUserDoc(string userId)
    {
        return _db.Collection("usuarios").Document(userId);
    }

    public async Task<UserStatsResponse>
        GetUserStats(string userId)
    {
        var flashcards =
            await GetUserDoc(userId)
                .Collection("flashcards")
                .GetSnapshotAsync();

        var resumos =
            await GetUserDoc(userId)
                .Collection("resumos")
                .GetSnapshotAsync();

        var pomodoros =
            await GetUserDoc(userId)
                .Collection("pomodoros")
                .GetSnapshotAsync();

        var eventos =
            await GetUserDoc(userId)
                .Collection("eventos")
                .GetSnapshotAsync();

        var userDoc =
            await GetUserDoc(userId)
                .GetSnapshotAsync();

        var data = userDoc.ToDictionary();

        int totalXp =
            data.ContainsKey("totalXp")
                ? Convert.ToInt32(data["totalXp"])
                : 0;

        int streak =
            data.ContainsKey("diasSequencia")
                ? Convert.ToInt32(data["diasSequencia"])
                : 0;

        return new UserStatsResponse
        {
            FlashcardsCriados = flashcards.Count,

            ResumosCriados = resumos.Count,

            PomodorosConcluidos = pomodoros.Count,

            EventosCriados = eventos.Count,

            DiasSequencia = streak,

            TotalAcoes =
                flashcards.Count +
                resumos.Count +
                pomodoros.Count +
                eventos.Count,

            TotalXp = totalXp
        };
    }

    public async Task<bool> HasAchievement(
        string userId,
        string achievementId)
    {
        var snapshot =
            await GetUserDoc(userId)
                .Collection("achievements")
                .Document(achievementId)
                .GetSnapshotAsync();

        return snapshot.Exists;
    }

    public async Task UnlockAchievement(
        string userId,
        UserAchievementDto achievement)
    {
        await GetUserDoc(userId)
            .Collection("achievements")
            .Document(achievement.Id)
            .SetAsync(achievement);
    }

    public async Task<List<UserAchievementDto>>
        GetUserAchievements(string userId)
    {
        var snapshot =
            await GetUserDoc(userId)
                .Collection("achievements")
                .GetSnapshotAsync();

        return snapshot.Documents
            .Select(doc =>
                doc.ConvertTo<UserAchievementDto>())
            .ToList();
    }
}