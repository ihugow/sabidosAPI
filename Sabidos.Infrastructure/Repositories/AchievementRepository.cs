using Google.Cloud.Firestore;
using sabidos.Application.DTOs;
using sabidos.Application.Interfaces;

namespace sabidos.Infrastructure.Repositories;

public class AchievementRepository : IAchievementRepository
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
            await _db.Collection("Flashcards")
                .WhereEqualTo("UserId", userId)
                .GetSnapshotAsync();

        var resumos =
            await _db.Collection("resumos")
                .WhereEqualTo("userId", userId)
                .GetSnapshotAsync();

        var pomodoros =
            await _db.Collection("pomodoros")
                .WhereEqualTo("userId", userId)
                .GetSnapshotAsync();

        var eventos =
            await _db.Collection("AgendaEvents")
                .WhereEqualTo("UserId", userId)
                .GetSnapshotAsync();

        var userDoc =
            await GetUserDoc(userId)
                .GetSnapshotAsync();

        var data = userDoc.Exists ? userDoc.ToDictionary() : new Dictionary<string, object>();

        int totalXp =
            data != null && data.ContainsKey("totalXp")
                ? Convert.ToInt32(data["totalXp"])
                : 0;

        int streak =
            data != null && data.ContainsKey("diasSequencia")
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