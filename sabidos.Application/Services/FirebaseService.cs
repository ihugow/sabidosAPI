using Google.Cloud.Firestore;

public class FirebaseService
{
    private readonly FirestoreDb _firestore;

    public FirebaseService()
    {
        _firestore = FirestoreDb.Create("seu-projeto-id");
    }

    public async Task UpdateUserPoints(string userId, int totalPoints)
    {
        var doc = _firestore.Collection("usuarios").Document(userId);

        await doc.SetAsync(new
        {
            points = totalPoints,
            updatedAt = Timestamp.GetCurrentTimestamp()
        }, SetOptions.MergeAll);
    }
}