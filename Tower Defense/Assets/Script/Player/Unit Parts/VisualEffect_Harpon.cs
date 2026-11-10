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
    private Transform linksParent;
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

        for (int i = 0; i < links.Count; i++)
        {
            if (i < activeLinksAmount)
            {
                Vector3 newPos = startPoint.position + direction * linkDistance * (i + 1);
                links[i].EnableLink(true, newPos);
            }
            else
            {
                links[i].EnableLink(false, Vector3.zero);
            }

            if (i != links.Count - 1)
            {
                links[i].UpdateLineRenderer(links[i], links[i + 1]);
            }
        }
    }

    private void InitializeLinks()
    {
        for (int i = 0; i < maxLinks; i++)
        {
            Projectile_HarponChainLink newLink = Instantiate(chainLinkPrefab, startPoint.position, Quaternion.identity, linksParent).GetComponent<Projectile_HarponChainLink>();
            links.Add(newLink);
        }
    }
}
