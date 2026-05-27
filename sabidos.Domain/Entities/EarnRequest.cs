using System;
using System.Text.Json;
public enum PointActionType
{
    // Resumo
    ResumoCriado,

    // Flashcard
    FlashcardRespondido,

    // Pomodoro
    PomodoroCompleto,

    // Agenda
    EventoCriado
}

public class EarnRequest
{
    public PointActionType Action { get; set; }

    public JsonElement Data { get; set; }
}
