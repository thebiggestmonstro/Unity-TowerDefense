using System;
using System.Collections;
using UnityEngine;
using CartoonFX;

public class PooledVFX : MonoBehaviour
{
    private ParticleSystem[] particleSystems;
    private Action<PooledVFX> release;
    private static readonly WaitForSeconds checkWait = new WaitForSeconds(0.2f);

    public void Initialize(Action<PooledVFX> releaseAction)
    {
        release = releaseAction;
        particleSystems = GetComponentsInChildren<ParticleSystem>(true);

        foreach (CFXR_Effect effect in GetComponentsInChildren<CFXR_Effect>(true))
        {
            effect.clearBehavior = CFXR_Effect.ClearBehavior.None;
        }
    }

    public void Play(Vector3 position, Quaternion rotation, float scale)
    {
        transform.SetPositionAndRotation(position, rotation);
        transform.localScale = Vector3.one * scale;
        gameObject.SetActive(true);

        foreach (ParticleSystem ps in particleSystems)
        {
            ps.Clear(true);
            ps.Play(true);
        }

        StartCoroutine(CoWaitAndRelease());
    }

    private IEnumerator CoWaitAndRelease()
    {
        yield return checkWait;

        foreach (ParticleSystem ps in particleSystems)
        {
            if (ps.main.loop)
            {
                ps.Stop(false, ParticleSystemStopBehavior.StopEmitting);
            }
        }

        while (particleSystems[0].IsAlive(true))
        {
            yield return checkWait;
        }

        release(this);
    }
}
