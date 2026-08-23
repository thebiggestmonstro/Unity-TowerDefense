using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class VisualEffect_AttackRadius : MonoBehaviour
{
    [SerializeField]
    private float lineWidth = .1f;
    [SerializeField]
    private float radiusValue;

    private LineRenderer lineRenderer;
    private int lineSegments = 50;

    private void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.positionCount = lineSegments + 1;
        lineRenderer.useWorldSpace = true;
        lineRenderer.startWidth = lineWidth;
        lineRenderer.endWidth = lineWidth;
    }

    public void SetMaterial(Material material) => lineRenderer.material = material;

    public void CreateCircle(bool bShowCircle, float radius = 0)
    {
        lineRenderer.enabled = bShowCircle;

        if (bShowCircle == false)
        {
            return;
        }

        float angle = 0;
        Vector3 center = transform.position;

        for (int i = 0; i < lineSegments; i++)
        {
            float x = Mathf.Sin(Mathf.Deg2Rad * angle) * radius;
            float z = Mathf.Cos(Mathf.Deg2Rad * angle) * radius;

            lineRenderer.SetPosition(i, new Vector3(x + center.x, center.y, z + center.z));
            angle += 360f / lineSegments;
        }

        lineRenderer.SetPosition(lineSegments, lineRenderer.GetPosition(0));
    }
}