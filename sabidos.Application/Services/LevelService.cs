namespace sabidos.Application.Services;

public class LevelService
{
    // Fórmula: XP necessário para chegar no próximo nível
    // Nível 1 -> 2: 100 XP
    // Nível 2 -> 3: ~230 XP (total acumulado)
    // Nível 3 -> 4: ~380 XP (total acumulado)
    public int CalculateLevel(int totalXp)
    {
        int level = 1;
        while (totalXp >= GetXpThreshold(level + 1))
        {
            level++;
        }
        return level;
    }

    // Retorna o XP total acumulado necessário para ATINGIR um determinado nível
    public int GetXpThreshold(int level)
    {
        if (level <= 1) return 0;

        // Base 100 XP para o nível 2, crescendo com expoente 1.3
        return (int)(100 * Math.Pow(level - 1, 1.3));
    }

    // Retorna quanto XP o usuário já tinha ao entrar no nível atual
    public int GetCurrentLevelBaseXp(int currentLevel)
    {
        return GetXpThreshold(currentLevel);
    }

    // Retorna quanto XP total é necessário para o PRÓXIMO nível
    public int GetNextLevelXpThreshold(int currentLevel)
    {
        return GetXpThreshold(currentLevel + 1);
    }
}