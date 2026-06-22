using UnityEngine;

public class AscendingParts : MonoBehaviour
{
    [SerializeField]
    private Transform ascendingParts;
    [SerializeField]
    private LayerMask groundMask;
    [SerializeField]
    private float verticalRotationSpeed;

    private void Update()
    {
        AlignWithSlope();
    }

    private void AlignWithSlope()
    {
        if (ascendingParts == null)
        {
            return;
        }

        if (Physics.Raycast(ascendingParts.position, Vector3.down, out RaycastHit hit, Mathf.Infinity, groundMask))
        {
            Quaternion targetRotation = Quaternion.FromToRotation(transform.up, hit.normal) * transform.rotation;
            ascendingParts.rotation = Quaternion.Slerp(ascendingParts.rotation, targetRotation, Time.deltaTime * verticalRotationSpeed);
        }
    }
}
