namespace sabidos.Application.DTOs;

public class DailyMissionDto
{
    public string Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public string Emoji { get; set; }
    public int Goal { get; set; }
    public int Progress { get; set; }
    public bool Completed { get; set; }
    public bool Claimed { get; set; }
    public string ActionType { get; set; }
    public int XpReward { get; set; }
}

public class DailyMissionsResponse
{
    public List<DailyMissionDto> Missions { get; set; }
    public int RerollsLeft { get; set; }
    public int TotalXp { get; set; }
    public int Level { get; set; }
    public int XpCurrentLevelBase { get; set; }
    public int XpNextLevelThreshold { get; set; }
}
