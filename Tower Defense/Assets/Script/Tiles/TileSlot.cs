using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor; 
#endif

public class TileSlot : MonoBehaviour
{
    public void SwitchTile(GameObject referencedTile)
    {
        TileSlot newTilePrefab = referencedTile.GetComponent<TileSlot>();

#if UNITY_EDITOR
        Undo.RecordObject(gameObject, "Switch Tile");

        if (GetMeshFilter != null)
        {
            Undo.RecordObject(GetMeshFilter, "Switch Tile Mesh");
        }
        if (GetMeshRenderer != null)
        { 
            Undo.RecordObject(GetMeshRenderer, "Switch Tile Material");
        }
#endif
        if (GetMeshFilter != null)
        {
            GetMeshFilter.sharedMesh = newTilePrefab.GetMesh();
        }
        if (GetMeshRenderer != null)
        {
            GetMeshRenderer.sharedMaterial = newTilePrefab.GetMaterial();
        }

        List<GameObject> currentChildren = GetAllChildren();
        for (int i = currentChildren.Count - 1; i >= 0; i--)
        {
#if UNITY_EDITOR
            Undo.DestroyObjectImmediate(currentChildren[i]);
#else
            Destroy(currentChildren[i]);
#endif
        }

        foreach (Transform childTransform in referencedTile.transform)
        {
#if UNITY_EDITOR
            GameObject spawnedChild = Instantiate(childTransform.gameObject, this.transform);

            if (spawnedChild != null)
            {
                spawnedChild.transform.localPosition = childTransform.localPosition;
                spawnedChild.transform.localRotation = childTransform.localRotation;
                spawnedChild.transform.localScale = childTransform.localScale;

                Undo.RegisterCreatedObjectUndo(spawnedChild, "Spawn Tile Child");
            }
#else
    Instantiate(childTransform.gameObject, transform);
#endif
        }

#if UNITY_EDITOR
        EditorUtility.SetDirty(gameObject);
#endif
    }

    public Material GetMaterial() => GetMeshRenderer != null ? GetMeshRenderer.sharedMaterial : null;
    public Mesh GetMesh() => GetMeshFilter != null ? GetMeshFilter.sharedMesh : null;

    public List<GameObject> GetAllChildren()
    {
        List<GameObject> children = new List<GameObject>();

        foreach (Transform child in transform)
        {
            children.Add(child.gameObject);
        }

        return children;
    }

    private MeshRenderer GetMeshRenderer => GetComponent<MeshRenderer>();
    private MeshFilter GetMeshFilter => GetComponent<MeshFilter>();
}
