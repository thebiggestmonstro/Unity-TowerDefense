using System.Collections;
using UnityEngine;

public class VisualEffect_Hammer : MonoBehaviour
{
    [Header("Hammer Settings")]
    [SerializeField]
    private Transform hammerTransform;
    [SerializeField]
    private Transform hammerHolderTransform;
    [Space]
    [SerializeField] 
    private Transform sideWireTransform;
    [SerializeField] 
    private Transform sideHandleTransform;

    [Space]
    [Header("Pull and Release Setting")]
    [SerializeField]
    private float attackOffsetY;
    [SerializeField]
    private float attackDuration;
    private float reloadDuration;

    [Space]
    [SerializeField] 
    private RotationParts valveTransform;
    [SerializeField] 
    private ParticleSystem[] hammerVFX;

    private Player_TowerHammer towerHammer;

    private void Awake()
    {
        towerHammer = GetComponent<Player_TowerHammer>();
        reloadDuration = towerHammer.GetAttackCooldown() - attackDuration;
    }

    public void PlayerHammerAttackVFX()
    {
        StartCoroutine(CoHammerAttack());
    }

    private IEnumerator CoHammerAttack()
    {
        valveTransform.AdjustRotationSpeed(25);
        StartCoroutine(CoChangePosition(hammerTransform, -attackOffsetY, attackDuration));
        StartCoroutine(CoChangeScale(hammerHolderTransform, 7.0f, attackDuration));
        StartCoroutine(CoChangePosition(sideHandleTransform, 0.45f, attackDuration));

        yield return new WaitForSeconds(attackDuration);
        PlayHammerVFX();

        valveTransform.AdjustRotationSpeed(3);
        StartCoroutine(CoChangePosition(hammerTransform, attackOffsetY, reloadDuration));
        StartCoroutine(CoChangeScale(hammerHolderTransform, 1.0f, reloadDuration));
        StartCoroutine(CoChangePosition(sideHandleTransform, -0.45f, reloadDuration));
    }

    private IEnumerator CoChangePosition(Transform inTransform, float inYOffset, float inDuration = 0.1f)
    {
        float time = 0;

        Vector3 initialPosition = inTransform.localPosition;
        Vector3 targetPosition = new Vector3(initialPosition.x, initialPosition.y + inYOffset, initialPosition.z);

        while (time < inDuration)
        {
            inTransform.localPosition = Vector3.Lerp(initialPosition, targetPosition, time / inDuration);
            time += Time.deltaTime;
            yield return null;
        }

        inTransform.localPosition = targetPosition;
    }

    private IEnumerator CoChangeScale(Transform inTransform, float newScale, float inDuration = 0.25f)
    {
        float time = 0;

        Vector3 initialScale = inTransform.localScale;
        Vector3 targetScale = new Vector3(initialScale.x, newScale, initialScale.z);

        while (time < inDuration)
        {
            inTransform.localScale = Vector3.Lerp(initialScale, targetScale, time / inDuration);
            time += Time.deltaTime;
            yield return null;
        }

        inTransform.localScale = targetScale;
    }

    private void PlayHammerVFX()
    {
        foreach (var vfx in hammerVFX)
        {
            vfx.Play();
        }
    }
}
