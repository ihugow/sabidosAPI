using Google.Cloud.Firestore;
using sabidos.Domain.Entities;
using sabidos.Domain.Interfaces;

namespace sabidos.Infrastructure.Repositories;

public class FirestoreFlashcardRepository : IFlashcardRepository
{
    private readonly FirestoreDb _firestoreDb;
    private readonly string _collectionName = "Flashcards";

    public FirestoreFlashcardRepository(FirestoreDb firestoreDb)
    {
        _firestoreDb = firestoreDb;
    }

    public async Task AddAsync(Flashcard flashcard)
    {
        var collection = _firestoreDb.Collection(_collectionName);
        var document = collection.Document(flashcard.Id);
        await document.SetAsync(flashcard);
    }

    public async Task<Flashcard?> GetByIdAsync(string id)
    {
        var document = _firestoreDb.Collection(_collectionName).Document(id);
        var snapshot = await document.GetSnapshotAsync();

        if (snapshot.Exists)
        {
            return snapshot.ConvertTo<Flashcard>();
        }

        return null;
    }

    public async Task<IEnumerable<Flashcard>> GetByUserIdAsync(string userId)
    {
        var query = _firestoreDb.Collection(_collectionName).WhereEqualTo("UserId", userId);
        var snapshot = await query.GetSnapshotAsync();

        var flashcards = new List<Flashcard>();
        foreach (var document in snapshot.Documents)
        {
            if (document.Exists)
            {
                flashcards.Add(document.ConvertTo<Flashcard>());
            }
        }

        return flashcards;
    }

    public async Task<IEnumerable<Flashcard>> GetByCollectionIdAsync(string collectionId)
    {
        var query = _firestoreDb.Collection(_collectionName).WhereEqualTo("CollectionId", collectionId);
        var snapshot = await query.GetSnapshotAsync();

        var flashcards = new List<Flashcard>();
        foreach (var document in snapshot.Documents)
        {
            if (document.Exists)
            {
                flashcards.Add(document.ConvertTo<Flashcard>());
            }
        }

        return flashcards;
    }

    public async Task UpdateAsync(Flashcard flashcard)
    {
        var document = _firestoreDb.Collection(_collectionName).Document(flashcard.Id);
        await document.SetAsync(flashcard, SetOptions.MergeAll);
    }

    public async Task DeleteAsync(string id)
    {
        var document = _firestoreDb.Collection(_collectionName).Document(id);
        await document.DeleteAsync();
    }

    public async Task DeleteByCollectionIdAsync(string collectionId)
    {
        var query = _firestoreDb.Collection(_collectionName).WhereEqualTo("CollectionId", collectionId);
        var snapshot = await query.GetSnapshotAsync();
        
        var batch = _firestoreDb.StartBatch();
        foreach (var document in snapshot.Documents)
        {
            batch.Delete(document.Reference);
        }
        await batch.CommitAsync();
    }
}
