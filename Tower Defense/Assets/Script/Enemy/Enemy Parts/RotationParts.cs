using UnityEngine;

public class RotationParts : MonoBehaviour
{
    [SerializeField]
    private Vector3 rotationVector;
    [SerializeField]
    private float rotationSpeed;

    private bool canRotate = true;

    private void Update()
    {
        if (!canRotate)
        {
            return;
        }

        float newRotationSpeed = rotationSpeed * 100;
        transform.Rotate(rotationVector * newRotationSpeed * Time.deltaTime);
    }

    public void StopRotation()
    {
        canRotate = false;
    }
}
