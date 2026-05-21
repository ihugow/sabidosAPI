using Google.Cloud.Firestore;

public class AchievementService
{
    private readonly FirestoreDb _db;

    public AchievementService(FirestoreDb db)
    {
        _db = db;
    }

    public async Task CheckAchievements(string userId, int totalPoints)
    {
        var achievements = new List<(string Name, int RequiredPoints)>
        {
            ("Iniciante", 100),
            ("Dedicado", 500),
            ("Mestre", 1000)
        };

        var userAchievementsRef = _db
            .Collection("usuarios")
            .Document(userId)
            .Collection("achievements");

        foreach (var achievement in achievements)
        {
            var existing = await userAchievementsRef
                .WhereEqualTo("name", achievement.Name)
                .GetSnapshotAsync();

            if (!existing.Documents.Any() && totalPoints >= achievement.RequiredPoints)
            {
                await userAchievementsRef.AddAsync(new
                {
                    name = achievement.Name,
                    unlockedAt = Timestamp.GetCurrentTimestamp()
                });
            }
        }
    }

}