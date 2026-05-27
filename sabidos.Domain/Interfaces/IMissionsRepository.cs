using sabidos.Domain.Entities;

namespace sabidos.Domain.Interfaces;

public interface IMissionsRepository
{
    Task<UserDailyState> GetUserDailyState(string userId);
    Task SaveUserDailyState(string userId, UserDailyState state);
}
