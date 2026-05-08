public class PointTransaction
{
    public Guid Id { get; set; }
    public string UserId { get; set; }
    public int Points { get; set; }
    public string ActionType { get; set; } // RESUMO, FLASHCARD, POMODORO
    public string ReferenceId { get; set; }
    public DateTime CreatedAt { get; set; }
}