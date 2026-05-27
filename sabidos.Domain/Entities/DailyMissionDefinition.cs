namespace sabidos.Domain.Entities;

public class DailyMissionDefinition
{
    public string Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public string Emoji { get; set; }
    public int Goal { get; set; }
    public PointActionType ActionType { get; set; }
    public int XpReward { get; set; }
}
