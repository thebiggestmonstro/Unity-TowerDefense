using UnityEngine;

public class Enemy_Stealth : Enemy_Base
{
    [Header("Stealth Details")]
    [SerializeField]
    private float stealthRadius = 1.0f;
    [SerializeField]
    private StealthParts stealthParts;
    [SerializeField]
    private ParticleSystem[] smokeVFXs;

    protected override void Awake()
    {
        base.Awake();

        if (stealthParts != null)
        {
            stealthParts.SetupStealth(stealthRadius);
        }
    }

    public override void AddReveal()
    {
        base.AddReveal();

        if (revealSourceCount == 1)
        {
            foreach (ParticleSystem vfx in smokeVFXs)
            {
                if (vfx != null)
                {
                    vfx.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
                }
            }
        }
    }

    public override void RemoveReveal()
    {
        base.RemoveReveal();

        if (revealSourceCount == 0)
        {
            foreach (ParticleSystem vfx in smokeVFXs)
            {
                if (vfx != null)
                {
                    vfx.Play();
                }
            }
        }
    }
}
