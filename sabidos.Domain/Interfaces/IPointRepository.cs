namespace sabidos.Domain.Interfaces;

public interface IPointRepository
{
    Task<int> GetUserPoints(string userId);
    Task UpdatePoints(string userId, int pointsToAdd);
    Task AddTransaction(string userId, int points, string action);
    Task<int> GetDailyActionCount(string userId, string action, DateTime date);
}
