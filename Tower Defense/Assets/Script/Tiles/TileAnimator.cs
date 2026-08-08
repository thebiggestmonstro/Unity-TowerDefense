using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileAnimator : MonoBehaviour
{
    [SerializeField]
    private float yMovementDuration = 0.1f;
    [SerializeField]
    private float buildTileSlotYOffset = 0.25f;

    public static TileAnimator Instance { get; private set; }
    private Dictionary<Transform, Coroutine> activeMoveTileCoroutines = new Dictionary<Transform, Coroutine>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void MoveTile(Transform tileToMove, Vector3 targetPosition)
    {
        StopTileMovement(tileToMove);
        Coroutine newCoroutine = StartCoroutine(CoMoveTile(tileToMove, targetPosition));
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

    private IEnumerator CoMoveTile(Transform tileToMove, Vector3 targetPosition)
    {
        float time = 0;
        Vector3 startPosition = tileToMove.position;
        while (time < yMovementDuration)
        {
            if (tileToMove == null)
            {
                yield break;
            }

            tileToMove.position = Vector3.Lerp(startPosition, targetPosition, time / yMovementDuration);
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
    public float GetMovementDurtaion() => yMovementDuration;

    public Coroutine GetActiveTileMovementCoroutine(Transform transform) => activeMoveTileCoroutines[transform] != null ? activeMoveTileCoroutines[transform] : null;
}