using UnityEngine;

public class Projectile_HarponChainLink : MonoBehaviour
{
    private LineRenderer lineRenderer;
    private MeshRenderer meshRenderer;
    private ParticleSystem vfx;

    private void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.positionCount = 2;

        meshRenderer = GetComponentInChildren<MeshRenderer>();
        vfx = GetComponentInChildren<ParticleSystem>();

        EnableLink(false, transform.position);
        EnableVFX(false);
    }

    public void EnableLink(bool enable, Vector3 newPos)
    { 
        meshRenderer.enabled = enable;
        transform.position = newPos;
    }

    public void UpdateLineRenderer(Projectile_HarponChainLink startPoint, Projectile_HarponChainLink endPoint)
    { 
        lineRenderer.enabled = startPoint.GetCurrentActive() && endPoint.GetCurrentActive();
        EnableVFX(lineRenderer.enabled);

        if (!lineRenderer.enabled)
        {
            return;
        }

        lineRenderer.SetPosition(0, startPoint.transform.position);
        lineRenderer.SetPosition(1, endPoint.transform.position);
    }

    public bool GetCurrentActive() => meshRenderer.enabled;

    public void EnableVFX(bool enable)
    {
        if (enable)
        {
            if (!vfx.isPlaying)
            {
                vfx.Play();
            }
        }
        else if (vfx.isPlaying)
        {
            vfx.Stop();
        }
    }
}
