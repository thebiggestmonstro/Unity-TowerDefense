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
    public Dictionary<Transform, Coroutine> activeMoveTileCoroutines = new Dictionary<Transform, Coroutine>();

    public Transform targetTile;

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

    [ContextMenu("Move Tile")]
    public void TestMoveTile()
    {
        Vector3 targetPosition = targetTile.position + new Vector3(0, 0.25f);
        MoveTile(targetTile, targetPosition);
    }


    public void MoveTile(Transform tileToMove, Vector3 targetPosition)
    {
        if (activeMoveTileCoroutines.TryGetValue(tileToMove, out Coroutine runningCoroutine))
        {
            if (runningCoroutine != null)
            {
                StopCoroutine(runningCoroutine);
            }
            activeMoveTileCoroutines.Remove(tileToMove);
        }

        Coroutine newCoroutine = StartCoroutine(CoMoveTile(tileToMove, targetPosition));
        activeMoveTileCoroutines[tileToMove] = newCoroutine;
    }


    private IEnumerator CoMoveTile(Transform tileToMove, Vector3 targetPosition)
    {
        float time = 0;
        Vector3 startPosition = tileToMove.position;

        while (time < yMovementDuration)
        { 
            tileToMove.position = Vector3.Lerp(startPosition, targetPosition, time / yMovementDuration);
            time += Time.deltaTime;
            yield return null;
        }

        tileToMove.position = targetPosition;

        if (activeMoveTileCoroutines.ContainsKey(tileToMove))
        {
            activeMoveTileCoroutines.Remove(tileToMove);
        }
    }

    public float GetBuildTileOffset() => buildTileSlotYOffset;
    public float GetMovementDurtaion() => yMovementDuration;
}
