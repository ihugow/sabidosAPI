using Google.Cloud.Firestore;
using sabidos.Domain.Entities;
using sabidos.Domain.Interfaces;

namespace sabidos.Infrastructure.Repositories;

public class FirestoreMissionsRepository : IMissionsRepository
{
    private readonly FirestoreDb _db;

    public FirestoreMissionsRepository(FirestoreDb db)
    {
        _db = db;
    }

    private DocumentReference GetStateDoc(string userId)
    {
        return _db.Collection("usuarios").Document(userId).Collection("dailyState").Document("status");
    }

    public async Task<UserDailyState> GetUserDailyState(string userId)
    {
        var snapshot = await GetStateDoc(userId).GetSnapshotAsync();
        if (!snapshot.Exists) return null;
        
        return snapshot.ConvertTo<UserDailyState>();
    }

    public async Task SaveUserDailyState(string userId, UserDailyState state)
    {
        await GetStateDoc(userId).SetAsync(state);
    }
}
