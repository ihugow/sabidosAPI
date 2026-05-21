using Google.Cloud.Firestore;

namespace sabidos.Domain.Entities
{
    [FirestoreData]
    public class AgendaEvent
    {
        [FirestoreDocumentId]
        public string Id { get; set; } = Guid.NewGuid().ToString();

        [FirestoreProperty]
        public string Title { get; set; } = string.Empty;

        private DateTime _date;
        [FirestoreProperty]
        public DateTime Date 
        { 
            get => _date; 
            set => _date = value.Kind == DateTimeKind.Unspecified 
                ? DateTime.SpecifyKind(value, DateTimeKind.Utc) 
                : value.ToUniversalTime(); 
        }

        [FirestoreProperty]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [FirestoreProperty]
        public string UserId { get; set; } = string.Empty;
    }
}
