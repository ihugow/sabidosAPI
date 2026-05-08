// using System;
// public enum PointActionType
// {
//     // Resumo
//     ResumoCriado,

//     // Flashcard
//     FlashcardRespondido,

//     // Pomodoro
//     PomodoroCompleto
// }
public class EarnRequest
{
    // public PointActionType Action { get; set; }
    public string Action { get; set; }
    public object Data { get; set; }
}