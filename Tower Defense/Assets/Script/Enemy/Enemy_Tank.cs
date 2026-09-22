using UnityEngine;

public class Enemy_Tank : Enemy_Base
{
    [Header("Tank Details")]
    [SerializeField]
    private float shieldAmount = 50.0f;
    [SerializeField]
    private TankParts tankShield;

    protected override void Awake()
    {
        base.Awake();

        if (tankShield != null)
        {
            tankShield.gameObject.SetActive(true);
            tankShield.SetupShield(shieldAmount);
        }
    }
}