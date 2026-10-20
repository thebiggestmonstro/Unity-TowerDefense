using System.Collections;
using UnityEngine;

public class VisulEffect_MachineGun : MonoBehaviour
{
    [Header("Recool Settings")]
    [SerializeField]
    private float recoilOffset = -0.2f;
    [SerializeField]
    private float recoverSpeed = 1.0f;
    [SerializeField]
    private ParticleSystem onAttackVFX;

    public void PlayRecoliVFX(Transform gunPoint)
    {
        PlayOnAttackVFX(gunPoint.position);
        StartCoroutine(CoRecoli(gunPoint));
    }

    private void PlayOnAttackVFX(Vector3 inPos)
    {
        onAttackVFX.transform.position = inPos;
        onAttackVFX.Play();
    }

    private IEnumerator CoRecoli(Transform gunPoint)
    {
        Transform objectToMove = gunPoint.transform.parent;
        Vector3 originalPosition = objectToMove.localPosition;
        Vector3 recoilPosition = originalPosition + new Vector3(0, 0, recoilOffset);

        objectToMove.localPosition = recoilPosition;

        while (objectToMove.localPosition != originalPosition)
        {
            objectToMove.localPosition = Vector3.MoveTowards(objectToMove.localPosition, originalPosition, recoverSpeed * Time.deltaTime);
            yield return null;
        }
    }
}
