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
    private int currency;

    public static GameManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        currentHp = maxHp;
        UIManager.GetUI<UI_InGame>("UI_InGame").UpdateHealthPointsText(currentHp, maxHp);

        currency = 10;
        UIManager.GetUI<UI_InGame>("UI_InGame").UpdateCurrencyText(currency);
    }

    public void UpdateHp(int value)
    {
        currentHp += value;
        UIManager.GetUI<UI_InGame>("UI_InGame").UpdateHealthPointsText(currentHp, maxHp);
    }

    public void UpdateCurrency(int value)
    {
        currency += value;
        UIManager.GetUI<UI_InGame>("UI_InGame").UpdateCurrencyText(currency);
    }
}