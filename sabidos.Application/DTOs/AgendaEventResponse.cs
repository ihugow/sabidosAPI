using System;

namespace sabidos.Application.DTOs
{
    public class AgendaEventResponse
    {
        public string Id { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public DateTime Date { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
