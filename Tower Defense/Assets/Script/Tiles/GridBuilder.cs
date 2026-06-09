using System.Collections.Generic;
using UnityEngine;

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

    [ContextMenu("Build Grid")]
    private void BuildGrid()
    {
        if (mainPrefab == null)
        {
            return;
        }

#if UNITY_EDITOR
        Undo.IncrementCurrentGroup();
        Undo.RecordObject(this, "Build Grid");
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

#if UNITY_EDITOR
        Undo.SetCurrentGroupName("Build Grid");
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
        Undo.RecordObject(this, "Clear Grid");
#endif
        ClearGridInternal();

        createdTiles.Clear();

#if UNITY_EDITOR
        Undo.SetCurrentGroupName("Clear Grid");
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
}