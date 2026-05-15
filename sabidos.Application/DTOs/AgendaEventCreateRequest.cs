using System;

namespace sabidos.Application.DTOs
{
    public class AgendaEventCreateRequest
    {
        public string Title { get; set; } = string.Empty;
        public DateTime Date { get; set; }
    }
}
