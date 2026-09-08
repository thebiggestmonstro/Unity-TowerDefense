using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("Player HP")]
    [SerializeField]
    private int maxHp;
    [SerializeField]
    private int currentHp;

    [Space]
    [Header("Currency")]
    [SerializeField]
    private int currency = 100;

    private void OnEnable()
    {
        GameEvents.OnEnemyReachedCastle += HandleEnemyReachedCastle;
        GameEvents.OnEnemyDefeated += HandleEnemyDefeated;
    }

    private void OnDisable()
    {
        GameEvents.OnEnemyReachedCastle -= HandleEnemyReachedCastle;
        GameEvents.OnEnemyDefeated -= HandleEnemyDefeated;
    }

    private void Start()
    {
        currentHp = maxHp;
        GameEvents.RaiseHealthChanged(currentHp, maxHp);
        GameEvents.RaiseCurrencyChanged(currency);
        GameEvents.RaiseLevelStarted();
    }

    private void HandleEnemyReachedCastle() => UpdateHp(-1);

    private void HandleEnemyDefeated(int rewardAmount) => UpdateCurrency(rewardAmount);

    public void UpdateHp(int value)
    {
        currentHp += value;
        GameEvents.RaiseHealthChanged(currentHp, maxHp);
        GameEvents.RaiseDamageTaken();

        if (currentHp <= 0)
        { 
            GameEvents.RaiseLevelLost();
        }
    }

    public void UpdateCurrency(int value)
    {
        currency += value;
        GameEvents.RaiseCurrencyChanged(currency);
    }

    public bool CheckEnoughCurrency(int price)
    {
        if (price <= currency)
        {
            currency -= price;
            GameEvents.RaiseCurrencyChanged(currency);
            return true;
        }

        GameEvents.RaiseCurrencyShortage();
        return false;
    }
}