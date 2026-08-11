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
    
    [Space]
    [Header("Grid Animation Settings")]
    [SerializeField]
    private float tileMoveDuration = 0.1f;
    [SerializeField]
    private float tileMoveDelay = 0.1f; 
    [SerializeField]
    private float yOffset = 5f;

    [Space]
    [Header("Main Scene")]
    [SerializeField]
    private GridBuilder mainSceneGrid;
    [SerializeField]
    private List<GameObject> mainMenuObjects = new List<GameObject>();

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
        CollectMainSceneObjects();
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
        List<GameObject> objectsToMove = GetObjectsToMove(gridToMove, showGrid);

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

    private List<GameObject> GetExtraObjects()
    {
        List<GameObject> extraObjects = new List<GameObject>();

        extraObjects.AddRange(WaveManager.Instance.GetActivePortals().Select(component => component.gameObject));
        extraObjects.AddRange(UnitManager.Instance.GetUnits<Player_Castle>().Select(component => component.gameObject));

        return extraObjects;
    }

    private List<GameObject> GetObjectsToMove(GridBuilder gridToMove, bool startOnTiles)
    { 
        List<GameObject> objectsToMove = new List<GameObject>();
        List<GameObject> extraObjects = GetExtraObjects();

        if (startOnTiles)
        {
            objectsToMove.AddRange(gridToMove.GetCreatedTiles());
            objectsToMove.AddRange(extraObjects);
        }
        else
        {
            objectsToMove.AddRange(extraObjects);
            objectsToMove.AddRange(gridToMove.GetCreatedTiles());
        }

        return objectsToMove;
    }

    private void CollectMainSceneObjects()
    {
        mainMenuObjects.AddRange(mainSceneGrid.GetCreatedTiles());
        mainMenuObjects.AddRange(GetExtraObjects());
    }

    public void EnableMainSceneObjects(bool enable)
    {
        foreach (var obj in mainMenuObjects)
        {
            obj.SetActive(enable);
        }
    }
}