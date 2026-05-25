namespace sabidos.Application.DTOs;

public class UserAchievementDto
{
    public string Id { get; set; }

    public string Title { get; set; }

    public string Description { get; set; }

    public bool Unlocked { get; set; }

    public int Progress { get; set; }

    public int Goal { get; set; }
}