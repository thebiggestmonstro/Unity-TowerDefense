using UnityEngine;

public class LegParts_Reference : MonoBehaviour
{
    [SerializeField]
    private LayerMask roadLayerMask;
    [SerializeField]
    private float contactPointRadius = 0.05f;

    public Vector3 GetContactPoint()
    {
        if (Physics.Raycast(transform.position, Vector3.down, out RaycastHit hitInfo, Mathf.Infinity, roadLayerMask))
        {
            return hitInfo.point;
        }

        return transform.position;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, GetContactPoint());
        Gizmos.DrawSphere(GetContactPoint(), contactPointRadius);
    }
}
