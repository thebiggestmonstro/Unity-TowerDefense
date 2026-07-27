using System.Collections.Generic;
using UnityEngine;
using Unity.AI.Navigation;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class GridBuilder : MonoBehaviour
{
    [SerializeField]
    private GameObject mainPrefab;
    [SerializeField]
    private int gridLength = 10;
    [SerializeField]
    private int gridWidth = 10;
    [SerializeField]
    private List<GameObject> createdTiles;

    private NavMeshSurface myNavMesh;

    private void Awake()
    {
        myNavMesh = GetComponent<NavMeshSurface>();
    }

#if UNITY_EDITOR
    private void OnEnable()
    {
        Undo.undoRedoPerformed += OnUndoRedoPerformed;
    }

    private void OnDisable()
    {
        Undo.undoRedoPerformed -= OnUndoRedoPerformed;
    }

    private void OnUndoRedoPerformed()
    {
        UpdateNavMesh();
    }
#endif

    [ContextMenu("Build Grid")]
    private void BuildGrid()
    {
        if (mainPrefab == null)
        {
            return;
        }

#if UNITY_EDITOR
        Undo.IncrementCurrentGroup();
        int undoGroupIndex = Undo.GetCurrentGroup(); 
        Undo.SetCurrentGroupName("Build Grid");
        Undo.RecordObject(this, "Build Grid State");
#endif
        ClearGridInternal();

        if (createdTiles == null)
        {
            createdTiles = new List<GameObject>();
        }
        else
        {
            createdTiles.Clear();
        }

        for (int x = 0; x < gridLength; x++)
        {
            for (int z = 0; z < gridWidth; z++)
            {
                CreateTile(x, z);
            }
        }

        UpdateNavMesh();

#if UNITY_EDITOR
        Undo.CollapseUndoOperations(undoGroupIndex);
        EditorUtility.SetDirty(gameObject);
#endif
    }

    [ContextMenu("Clear Grid")]
    private void ClearGrid()
    {
        if (createdTiles == null || createdTiles.Count == 0)
        {
            return;
        }

#if UNITY_EDITOR
        Undo.IncrementCurrentGroup();
        int undoGroupIndex = Undo.GetCurrentGroup(); 
        Undo.SetCurrentGroupName("Clear Grid");
        Undo.RecordObject(this, "Clear Grid State");
#endif
        ClearGridInternal();
        createdTiles.Clear();
        ClearNavMesh();

#if UNITY_EDITOR
        Undo.CollapseUndoOperations(undoGroupIndex);
        EditorUtility.SetDirty(gameObject);
#endif
    }

    private void ClearGridInternal()
    {
        if (createdTiles == null)
        {
            return;
        }

        for (int i = createdTiles.Count - 1; i >= 0; i--)
        {
            GameObject tile = createdTiles[i];
            if (tile != null)
            {
#if UNITY_EDITOR
                Undo.DestroyObjectImmediate(tile);
#else
                DestroyImmediate(tile);
#endif
            }
        }
    }

    private void CreateTile(float xPosition, float zPosition)
    {
        Vector3 newPosition = new Vector3(xPosition, 0, zPosition);
        GameObject newTile = Instantiate(mainPrefab, newPosition, Quaternion.identity, transform);

        if (newTile != null)
        {
            createdTiles.Add(newTile);

#if UNITY_EDITOR
            Undo.RegisterCreatedObjectUndo(newTile, "Create Tile Instance");
#endif
        }
    }

    private void UpdateNavMesh()
    {
        if (GetNavMeshSurface != null)
        {
            GetNavMeshSurface.BuildNavMesh();
        }
    }

    private void ClearNavMesh()
    {
        if (GetNavMeshSurface != null)
        {
            GetNavMeshSurface.RemoveData();
        }
    }

    private NavMeshSurface GetNavMeshSurface => GetComponent<NavMeshSurface>();
    public NavMeshSurface GetNavMesh() => myNavMesh;

    public List<GameObject> GetCreatedTiles() => createdTiles;
}