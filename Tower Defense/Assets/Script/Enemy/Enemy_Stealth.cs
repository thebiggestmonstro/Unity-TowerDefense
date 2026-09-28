using UnityEngine;

public class Enemy_Stealth : Enemy_Base
{
    [Header("Stealth Details")]
    [SerializeField]
    private float stealthRadius = 1.0f;
    [SerializeField]
    private StealthParts stealthParts;

    protected override void Awake()
    {
        base.Awake();

        if (stealthParts != null)
        {
            stealthParts.SetupStealth(stealthRadius);
        }
    }
}
