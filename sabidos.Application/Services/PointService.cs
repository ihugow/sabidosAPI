
//using sabidos.Domain.Enums;

public class PointService
{
    public int CalculatePoints(
        PointActionType action,
        object data
    )
    {
        return action switch
        {
            PointActionType.ResumoCriado => 10,

            PointActionType.FlashcardRespondido =>
                CalculateFlashcardPoints((FlashcardData)data),

            PointActionType.PomodoroCompleto =>
                CalculatePomodoroPoints((PomodoroData)data),

            PointActionType.EventoCriado => 5,

            _ => 0
        };
    }

    private int CalculateFlashcardPoints(FlashcardData data)
    {
       
        return data.Score >= 80 ? 20 : 5;

    }

    private int CalculatePomodoroPoints(
        PomodoroData data
    )
    {
        return data.CyclesCompleted * 10;
    }

}