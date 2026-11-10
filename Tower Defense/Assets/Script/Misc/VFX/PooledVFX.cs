using System;
using System.Collections;
using UnityEngine;

public class PooledVFX : MonoBehaviour
{
    private ParticleSystem[] particleSystems;
    private Action<PooledVFX> release;

    public void Initialize(Action<PooledVFX> releaseAction)
    {
        release = releaseAction;
        particleSystems = GetComponentsInChildren<ParticleSystem>(true);
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
        yield return 0.2f;

        while (particleSystems[0].IsAlive(true))
        {
            yield return 0.2f;
        }

        release(this);
    }
}
