using sabidos.Application.DTOs;

namespace sabidos.Application.Interfaces
{
    public interface IAchievementRepository
    {
        Task<UserStatsResponse> GetUserStats(string userId);
        Task<bool> HasAchievement(string userId, string achievementId);
        Task UnlockAchievement(string userId, UserAchievementDto achievement);
        Task<List<UserAchievementDto>> GetUserAchievements(string userId);
    }
}
