using Supabase.Postgrest.Models;
using Supabase.Postgrest.Attributes;

namespace sabidos.Domain.Entities
{
    [Table("Flashcard")]
    public class Flashcard : BaseModel
    {

        [PrimaryKey("id", false)]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Column("Front")]
        public string Front { get; set; } = string.Empty;

        [Column("Back")]
        public string Back { get; set; } = string.Empty;

        [Column("CreatedAt")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [Column("UserId")]
        public string UserId { get; set; } = string.Empty;
    }
}   
