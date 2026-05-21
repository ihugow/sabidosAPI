using System;
using System.Text.Json;
public enum PointActionType
{
    // Resumo
    ResumoCriado,

    // Flashcard
    FlashcardRespondido,

    // Pomodoro
    PomodoroCompleto
}

public class EarnRequest
{
    public PointActionType Action { get; set; }

    public JsonElement Data { get; set; }
}
