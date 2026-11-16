using System.Collections.Generic;
using UnityEngine;

public class VisualEffect_Harpon : MonoBehaviour
{
    [SerializeField]
    private Transform startPoint;
    [SerializeField]
    private Transform endPoint;
    [Space]
    [SerializeField]
    private GameObject chainLinkPrefab;
    [SerializeField]
    private float linkDistance = 0.1f;
    [SerializeField]
    private int maxLinks = 100;
    [SerializeField]
    private GameObject onElectroVFX;
    [SerializeField]
    private Vector3 electroVFXOffset;

    private List<Projectile_HarponChainLink> links = new List<Projectile_HarponChainLink>();
    private GameObject currentVFX;

    private void Start()
    {
        InitializeLinks();
    }

    private void OnDisable()
    {
        VFXManager vfxManager = GameServices.Get<VFXManager>();

        foreach (Projectile_HarponChainLink link in links)
        {
            vfxManager?.Release(chainLinkPrefab, link.gameObject);
        }

        links.Clear();
    }

    private void Update()
    {
        if (endPoint == null || endPoint == startPoint)
        {
            return;
        }

        ActivateLinks();
    }

    public void EnableChainVisuals(bool enable, Transform inEndPoint = null)
    {
        if (enable)
        {
            endPoint = inEndPoint;
        }
        else
        {
            endPoint = startPoint;
            ActivateLinks();
            DestroyVFX();
        }
    }

    public void CreateVFX(Transform targetTransform)
    {
        DestroyVFX();
        currentVFX = Instantiate(onElectroVFX, targetTransform.position + electroVFXOffset, Quaternion.identity, targetTransform);
    }

    private void DestroyVFX()
    {
        if (currentVFX != null)
        {
            Destroy(currentVFX);
        }
    }

    private void ActivateLinks()
    {
        Vector3 direction = (endPoint.position - startPoint.position).normalized;
        float distance = Vector3.Distance(startPoint.position, endPoint.position);
        int activeLinksAmount = Mathf.Min(maxLinks, Mathf.CeilToInt(distance / linkDistance));

        VFXManager vfxManager = GameServices.Get<VFXManager>();

        if (vfxManager == null)
        {
            return;
        }

        while (links.Count < activeLinksAmount)
        {
            links.Add(vfxManager.Get(chainLinkPrefab).GetComponent<Projectile_HarponChainLink>());
        }

        while (links.Count > activeLinksAmount)
        {
            int lastIndex = links.Count - 1;
            vfxManager.Release(chainLinkPrefab, links[lastIndex].gameObject);
            links.RemoveAt(lastIndex);
        }

        for (int i = 0; i < links.Count; i++)
        {
            links[i].EnableLink(true, startPoint.position + direction * linkDistance * (i + 1));
        }

        for (int i = 0; i < links.Count; i++)
        {
            links[i].UpdateLineRenderer(links[i], i < links.Count - 1 ? links[i + 1] : null);
        }
    }

    private void InitializeLinks()
    {
        GameServices.Get<VFXManager>()?.Prewarm(chainLinkPrefab, maxLinks);
    }
}