namespace sabidos.Application.DTOs;

public class UserGamificationProfile
{
    public UserStatsResponse Stats { get; set; }

    public List<UserAchievementDto> Achievements { get; set; }

    public int TotalXp { get; set; }

    public int Level { get; set; }

    public int XpCurrentLevelBase { get; set; }

    public int XpNextLevelThreshold { get; set; }
}