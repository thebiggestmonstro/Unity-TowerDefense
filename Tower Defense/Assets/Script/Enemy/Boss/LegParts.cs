using System.Collections;
using UnityEngine;

public class LegParts : MonoBehaviour
{
    [Header("Boss Part")]
    [SerializeField]
    private Transform actualTarget;
    [SerializeField]
    private LegParts_Reference legPartRef;
    [SerializeField]
    private Transform bottomLegTransform;
    [SerializeField]
    private Vector3 placeOffset;
    [SerializeField]
    private LegParts oppositeLeg;

    [Space]
    [Header("Move Option")]
    [SerializeField]
    private float moveThreshold = 0.45f;
    [SerializeField]
    private bool bShouldMove;
    [SerializeField]
    private float legMoveSpeed = 2.5f;

    private BossParts bossPart;
    private Transform worldTargetRefTransform;
    private bool canMove = true;
    private Coroutine currentMoveCoroutine;

    private void Awake()
    {
        bossPart = GetComponentInParent<BossParts>();
        worldTargetRefTransform = new GameObject(legPartRef.gameObject.name + "_world").transform;
        worldTargetRefTransform.position = actualTarget.position;
    }

    private IEnumerator CoMoveLeg()
    {
        oppositeLeg.CanMove(false);

        Vector3 contactPoint = legPartRef.GetContactPoint();
        while (Vector3.Distance(worldTargetRefTransform.position, contactPoint) > 0.01f)
        {
            worldTargetRefTransform.position = Vector3.MoveTowards(worldTargetRefTransform.position, contactPoint, bossPart.legMoveSpeed * Time.deltaTime);

            yield return null;
            contactPoint = legPartRef.GetContactPoint();
        }

        oppositeLeg.CanMove(true);
        currentMoveCoroutine = null;
    }

    public void CanMove(bool bCanMove) => canMove = bCanMove;

    public void UpdateLeg()
    {
        actualTarget.position = worldTargetRefTransform.position + new Vector3(0, placeOffset.y, 0);

        if (bottomLegTransform)
        {
            bottomLegTransform.forward = Vector3.down;
        }

        if (!canMove || currentMoveCoroutine != null)
        {
            return;
        }

        if (Vector3.Distance(worldTargetRefTransform.position, legPartRef.GetContactPoint()) > moveThreshold)
        {
            currentMoveCoroutine = StartCoroutine(CoMoveLeg());
        }
    }
}
