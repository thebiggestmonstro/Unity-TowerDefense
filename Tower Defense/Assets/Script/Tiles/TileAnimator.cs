using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileAnimator : MonoBehaviour
{
    [SerializeField]
    private float defaultYMovementDuration = 0.1f;
    [SerializeField]
    private float buildTileSlotYOffset = 0.25f;
    
    [Space]
    [Header("Grid Animation Settings")]
    [SerializeField]
    private GridBuilder mainSceneGrid;
    [SerializeField]
    private float tileMoveDuration = 0.1f;
    [SerializeField]
    private float tileMoveDelay = 0.1f; 
    [SerializeField]
    private float yOffset = 5f;

    private bool bIsGridMoving;

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

    private void Start()
    {
        ShowGrid(mainSceneGrid, true);
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

    public Coroutine GetActiveTileMovementCoroutine(Transform transform) => activeMoveTileCoroutines[transform] != null ? activeMoveTileCoroutines[transform] : null;

    private void ApplyOffset(List<GameObject> objectsToMove, Vector3 offset)
    {
        foreach (var obj in objectsToMove)
        {
            obj.transform.position += offset;
        }
    }

    public void ShowGrid(GridBuilder gridToMove, bool showGrid)
    {
        List<GameObject> objectsToMove = gridToMove.GetCreatedTiles();

        if (gridToMove.IsOnFirstLoad())
        {
            ApplyOffset(objectsToMove, new Vector3(0, -yOffset, 0));
        }

        float offset = showGrid ? yOffset : -yOffset;
        StartCoroutine(CoShowGrid(objectsToMove, yOffset));
    }

    private IEnumerator CoShowGrid(List<GameObject> objectsToMove, float targetYOffset)
    {
        bIsGridMoving = true;

        for (int i = 0; i < objectsToMove.Count; i++)
        {
            yield return new WaitForSeconds(tileMoveDelay);

            if (objectsToMove[i] == null)
            {
                continue;
            }

            Transform tile = objectsToMove[i].transform;
            Vector3 targetPosition = tile.position + new Vector3(0, targetYOffset, 0);
            MoveTile(tile, targetPosition, tileMoveDuration);
        }

        bIsGridMoving = false;
    }

    public void ShowUpMainGrid(bool showMainGrid)
    {
        ShowGrid(mainSceneGrid, showMainGrid);
    }

    public bool GetIsGridMoving() => bIsGridMoving;
}