using System;
using System.Collections.Generic;
using System.Text;

namespace sabidos.Application.DTOs
{
    public class FlashcardResponse
    {
        public Guid Id { get; set; }
        public string Front { get; set; } = string.Empty;
        public string Back { get; set; } = string.Empty;
        public string UserId { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
    }
}
