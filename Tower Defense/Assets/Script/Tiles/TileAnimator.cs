using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class TileAnimator : MonoBehaviour
{
    [SerializeField]
    private float defaultYMovementDuration = 0.1f;
    [SerializeField]
    private float buildTileSlotYOffset = 0.25f;

    private Dictionary<Transform, Coroutine> activeMoveTileCoroutines = new Dictionary<Transform, Coroutine>();

    private void OnEnable()
    {
        GameServices.Register(this);
    }

    private void OnDisable()
    {
        GameServices.Unregister(this);
    }

    public void MoveTile(Transform tileToMove, Vector3 targetPosition, float? newTileMoveDuration = null)
    {
        StopTileMovement(tileToMove);
        float duration = newTileMoveDuration ?? defaultYMovementDuration;
        Coroutine newCoroutine = StartCoroutine(CoMoveTile(tileToMove, targetPosition, duration));
        activeMoveTileCoroutines[tileToMove] = newCoroutine;
    }

    public void StopTileMovement(Transform tileToMove)
    {
        if (activeMoveTileCoroutines.TryGetValue(tileToMove, out Coroutine runningCoroutine))
        {
            if (runningCoroutine != null)
            {
                StopCoroutine(runningCoroutine);
            }
            activeMoveTileCoroutines.Remove(tileToMove);
        }
    }

    private IEnumerator CoMoveTile(Transform tileToMove, Vector3 targetPosition, float? newDuration = null)
    {
        float time = 0;
        Vector3 startPosition = tileToMove.position;
        float duration = newDuration ?? defaultYMovementDuration;

        while (time < duration)
        {
            if (tileToMove == null)
            {
                yield break;
            }

            tileToMove.position = Vector3.Lerp(startPosition, targetPosition, time / duration);
            time += Time.deltaTime;
            yield return null;
        }

        if (tileToMove == null)
        {
            yield break;
        }

        tileToMove.position = targetPosition;
        if (activeMoveTileCoroutines.ContainsKey(tileToMove))
        {
            activeMoveTileCoroutines.Remove(tileToMove);
        }
    }

    public float GetBuildTileOffset() => buildTileSlotYOffset;
    public float GetMovementDurtaion() => defaultYMovementDuration;

    public Coroutine GetActiveTileMovementCoroutine(Transform transform) =>
        activeMoveTileCoroutines.TryGetValue(transform, out Coroutine coroutine) ? coroutine : null;
}