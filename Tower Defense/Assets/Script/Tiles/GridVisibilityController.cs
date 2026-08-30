using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GridVisibilityController : MonoBehaviour
{
    [SerializeField]
    private TileAnimator tileAnimator;

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

    private void OnEnable()
    {
        GameServices.Register(this);
    }

    private void OnDisable()
    {
        GameServices.Unregister(this);
    }

    private void Start()
    {
        CollectMainSceneObjects();
        ShowGrid(mainSceneGrid, true);
    }

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
            tileAnimator.MoveTile(tile, targetPosition, tileMoveDuration);
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

        extraObjects.AddRange(GameServices.Get<EnemySpawner>().GetActivePortals().Select(component => component.gameObject));
        extraObjects.AddRange(GameServices.Get<UnitManager>().GetUnits<Player_Castle>().Select(component => component.gameObject));

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
