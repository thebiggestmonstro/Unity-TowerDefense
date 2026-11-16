using System;
using System.Collections;
using UnityEngine;

public class PooledVFX : MonoBehaviour
{
    private ParticleSystem[] particleSystems;
    private Action<PooledVFX> release;
    private static readonly WaitForSeconds checkWait = new WaitForSeconds(0.2f);

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
        yield return checkWait;

        while (particleSystems[0].IsAlive(true))
        {
            yield return checkWait;
        }

        release(this);
    }
}
