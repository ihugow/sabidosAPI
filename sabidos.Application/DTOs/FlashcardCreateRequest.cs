namespace sabidos.Application.DTOs
{
    public class FlashcardCreateRequest
    {
        public string CollectionId { get; set; } = string.Empty;
        public string Front { get; set; } = string.Empty;
        public string Back { get; set; } = string.Empty;
    }
}
