public class PointService
{
    public int CalculatePoints(string action, object data)
    {
        return action switch
        {
            "RESUMO" => 10,

            "FLASHCARD" => CalculateFlashcardPoints((FlashcardData)data),

            "POMODORO" => CalculatePomodoroPoints((PomodoroData)data),

            _ => 0
        };
    }

    private int CalculateFlashcardPoints(FlashcardData data)
    {
        int basePoints = data.Correct ? 5 : 2;

        return data.Difficulty switch
        {
            "FACIL" => basePoints,
            "MEDIO" => basePoints * 2,
            "DIFICIL" => basePoints * 3,
            _ => basePoints
        };
    }

    private int CalculatePomodoroPoints(PomodoroData data)
    {
        return data.CyclesCompleted * 10;
    }
}