using System;

public static class GameEvents
{
    // Player Status broadcasts
    public static event Action<int, int> OnHealthChanged;
    public static event Action OnDamageTaken;
    public static event Action<int> OnCurrencyChanged;
    public static event Action OnCurrencyShortage;

    // Gameplay Status requests
    public static event Action OnEnemyReachedCastle;
    public static event Action<int> OnEnemyDefeated;

    // Wave progress broadcasts
    public static event Action<bool> OnWaveTimerVisibilityChanged;
    public static event Action<float> OnWaveTimerUpdated;

    public static void RaiseHealthChanged(int currentHp, int maxHp) => OnHealthChanged?.Invoke(currentHp, maxHp);
    public static void RaiseDamageTaken() => OnDamageTaken?.Invoke();
    public static void RaiseCurrencyChanged(int currency) => OnCurrencyChanged?.Invoke(currency);
    public static void RaiseCurrencyShortage() => OnCurrencyShortage?.Invoke();
    public static void RaiseEnemyReachedCastle() => OnEnemyReachedCastle?.Invoke();
    public static void RaiseEnemyDefeated(int rewardAmount) => OnEnemyDefeated?.Invoke(rewardAmount);
    public static void RaiseWaveTimerVisibilityChanged(bool visible) => OnWaveTimerVisibilityChanged?.Invoke(visible);
    public static void RaiseWaveTimerUpdated(float remainingSeconds) => OnWaveTimerUpdated?.Invoke(remainingSeconds);
}
