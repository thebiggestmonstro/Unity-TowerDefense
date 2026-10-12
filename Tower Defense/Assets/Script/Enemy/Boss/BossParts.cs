using UnityEngine;

public class BossParts : MonoBehaviour
{
    [Header("Details")]
    public float legMoveSpeed = 3f;

    private LegParts[] legs;

    private void Start()
    {
        legs = GetComponentsInChildren<LegParts>();
    }

    private void Update()
    {
        foreach (var leg in legs)
        {
            leg.UpdateLeg();
        }
    }
}
