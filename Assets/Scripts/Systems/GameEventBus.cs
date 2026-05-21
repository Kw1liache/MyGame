using System;

public static class GameEventBus
{
    public static Action OnPlayerDeath;
    public static Action OnPlayerRespawn;
    public static Action OnEnemyKilled;
}