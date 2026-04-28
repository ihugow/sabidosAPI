using Google.Cloud.Firestore;

namespace sabidos.Domain.Entities
{
    [FirestoreData]
    public class FlashcardCollection
    {
        [FirestoreDocumentId]
        public string Id { get; set; } = Guid.NewGuid().ToString();

        [FirestoreProperty]
        public string Name { get; set; } = string.Empty;

        [FirestoreProperty]
        public string Color { get; set; } = string.Empty;

        [FirestoreProperty]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [FirestoreProperty]
        public string UserId { get; set; } = string.Empty;
    }
}
