using UnityEngine;

public class SwingParts : MonoBehaviour
{
    [Header("Swing Settings")]
    [SerializeField]
    private Vector3 swingAxis;
    [SerializeField]
    private float swingDegree = 10.0f;
    [SerializeField]
    private float swingSpeed = 1.0f;

    private Quaternion startRotation;

    private void Start()
    {
        startRotation = transform.rotation;
    }

    private void Update()
    {
        float angle = Mathf.Sin(Time.time * swingSpeed) * swingDegree;
        transform.localRotation = startRotation * Quaternion.AngleAxis(angle, swingAxis.normalized);
    }
}
