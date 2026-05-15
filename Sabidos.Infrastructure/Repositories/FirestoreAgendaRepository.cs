using Google.Cloud.Firestore;
using sabidos.Domain.Entities;
using sabidos.Domain.Interfaces;

namespace sabidos.Infrastructure.Repositories;

public class FirestoreAgendaRepository : IAgendaRepository
{
    private readonly FirestoreDb _firestoreDb;
    private readonly string _collectionName = "AgendaEvents";

    public FirestoreAgendaRepository(FirestoreDb firestoreDb)
    {
        _firestoreDb = firestoreDb;
    }

    public async Task AddAsync(AgendaEvent agendaEvent)
    {
        var collection = _firestoreDb.Collection(_collectionName);
        var document = collection.Document(agendaEvent.Id);
        await document.SetAsync(agendaEvent);
    }

    public async Task<AgendaEvent?> GetByIdAsync(string id)
    {
        var document = _firestoreDb.Collection(_collectionName).Document(id);
        var snapshot = await document.GetSnapshotAsync();

        if (snapshot.Exists)
        {
            return snapshot.ConvertTo<AgendaEvent>();
        }

        return null;
    }

    public async Task<IEnumerable<AgendaEvent>> GetByUserIdAsync(string userId)
    {
        var query = _firestoreDb.Collection(_collectionName).WhereEqualTo("UserId", userId);
        var snapshot = await query.GetSnapshotAsync();

        var events = new List<AgendaEvent>();
        foreach (var document in snapshot.Documents)
        {
            if (document.Exists)
            {
                events.Add(document.ConvertTo<AgendaEvent>());
            }
        }

        return events.OrderBy(e => e.Date);
    }

    public async Task UpdateAsync(AgendaEvent agendaEvent)
    {
        var document = _firestoreDb.Collection(_collectionName).Document(agendaEvent.Id);
        await document.SetAsync(agendaEvent, SetOptions.MergeAll);
    }

    public async Task DeleteAsync(string id)
    {
        var document = _firestoreDb.Collection(_collectionName).Document(id);
        await document.DeleteAsync();
    }
}
