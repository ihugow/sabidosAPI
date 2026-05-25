namespace sabidos.Application.Services;

public class LevelService
{
    public int CalculateLevel(int xp)
    {
        int level = 1;

        while (xp >= RequiredXp(level))
        {
            level++;
        }

        return level;
    }

    public int RequiredXp(int level)
    {
        return (int)(100 * Math.Pow(level, 1.5));
    }
}