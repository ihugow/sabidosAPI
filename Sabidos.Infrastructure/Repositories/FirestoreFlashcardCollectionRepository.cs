using Google.Cloud.Firestore;
using sabidos.Domain.Entities;
using sabidos.Domain.Interfaces;

namespace sabidos.Infrastructure.Repositories;

public class FirestoreFlashcardCollectionRepository : IFlashcardCollectionRepository
{
    private readonly FirestoreDb _firestoreDb;
    private readonly string _collectionName = "FlashcardCollections";

    public FirestoreFlashcardCollectionRepository(FirestoreDb firestoreDb)
    {
        _firestoreDb = firestoreDb;
    }

    public async Task AddAsync(FlashcardCollection collectionModel)
    {
        var collection = _firestoreDb.Collection(_collectionName);
        var document = collection.Document(collectionModel.Id);
        await document.SetAsync(collectionModel);
    }

    public async Task<FlashcardCollection?> GetByIdAsync(string id)
    {
        var document = _firestoreDb.Collection(_collectionName).Document(id);
        var snapshot = await document.GetSnapshotAsync();

        if (snapshot.Exists)
        {
            return snapshot.ConvertTo<FlashcardCollection>();
        }

        return null;
    }

    public async Task<IEnumerable<FlashcardCollection>> GetByUserIdAsync(string userId)
    {
        var query = _firestoreDb.Collection(_collectionName).WhereEqualTo("UserId", userId);
        var snapshot = await query.GetSnapshotAsync();

        var collections = new List<FlashcardCollection>();
        foreach (var document in snapshot.Documents)
        {
            if (document.Exists)
            {
                collections.Add(document.ConvertTo<FlashcardCollection>());
            }
        }

        return collections;
    }

    public async Task UpdateAsync(FlashcardCollection collectionModel)
    {
        var document = _firestoreDb.Collection(_collectionName).Document(collectionModel.Id);
        await document.SetAsync(collectionModel, SetOptions.MergeAll);
    }

    public async Task DeleteAsync(string id)
    {
        var document = _firestoreDb.Collection(_collectionName).Document(id);
        await document.DeleteAsync();
    }
}
