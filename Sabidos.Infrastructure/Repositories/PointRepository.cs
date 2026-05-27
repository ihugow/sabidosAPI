using Google.Cloud.Firestore;
using sabidos.Domain.Interfaces;

namespace sabidos.Infrastructure.Repositories;

public class PointRepository : IPointRepository
{
    private readonly FirestoreDb _db;

    public PointRepository(FirestoreDb db)
    {
        _db = db;
    }

    private DocumentReference GetUserDoc(string userId)
    {

        return _db.Collection("usuarios").Document(userId);

  }

    public async Task<int> GetUserPoints(string userId)
    {
        var doc = await GetUserDoc(userId).GetSnapshotAsync();

        if (!doc.Exists) return 0;

        return doc.ContainsField("totalXp")
            ? doc.GetValue<int>("totalXp")
            : 0;
    }

    public async Task UpdatePoints(string userId, int pointsToAdd)
    {
        var userRef = GetUserDoc(userId);

        await _db.RunTransactionAsync(async transaction =>
        {
            var snapshot = await transaction.GetSnapshotAsync(userRef);

            int current = snapshot.Exists && snapshot.ContainsField("totalXp")
                ? snapshot.GetValue<int>("totalXp")
                : 0;

            transaction.Set(userRef, new
            {
                totalXp = current + pointsToAdd,
                updatedAt = Timestamp.GetCurrentTimestamp()
            }, SetOptions.MergeAll);
        });
    }

    public async Task AddTransaction(string userId, int points, string action)
    {
        var collection = GetUserDoc(userId).Collection("transactions");

        await collection.AddAsync(new
        {
            points,
            action,
            createdAt = Timestamp.GetCurrentTimestamp()
        });
    }

    public async Task<int> GetDailyActionCount(string userId, string action, DateTime date)
    {
        var startOfDay = Timestamp.FromDateTime(date.Date.ToUniversalTime());
        var endOfDay = Timestamp.FromDateTime(date.Date.AddDays(1).ToUniversalTime());

        var snapshot = await GetUserDoc(userId)
            .Collection("transactions")
            .WhereEqualTo("action", action)
            .WhereGreaterThanOrEqualTo("createdAt", startOfDay)
            .WhereLessThan("createdAt", endOfDay)
            .GetSnapshotAsync();

        return snapshot.Count;
    }
}

