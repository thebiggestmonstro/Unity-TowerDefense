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
        Undo.RecordObject(transform, "Switch Tile Transform");

        if (GetMeshFilter != null)
        {
            Undo.RecordObject(GetMeshFilter, "Switch Tile Mesh");
        }
        if (GetMeshRenderer != null)
        { 
            Undo.RecordObject(GetMeshRenderer, "Switch Tile Material");
        }
        if (GetTileCollider != null)
        {
            Undo.RecordObject(GetTileCollider, "Switch Tile Collider");
        }
#endif
        gameObject.name = referencedTile.name;

        if (GetMeshFilter != null)
        {
            GetMeshFilter.sharedMesh = newTilePrefab.GetMesh();
        }
        if (GetMeshRenderer != null)
        {
            GetMeshRenderer.sharedMaterial = newTilePrefab.GetMaterial();
        }

        UpdateCollider(newTilePrefab.GetCollider());

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
            GameObject spawnedChild = Instantiate(childTransform.gameObject, transform);
            spawnedChild.transform.localPosition = childTransform.localPosition;
            spawnedChild.transform.localRotation = childTransform.localRotation;
            spawnedChild.transform.localScale = childTransform.localScale;
#endif
        }

#if UNITY_EDITOR
        EditorUtility.SetDirty(gameObject);
#endif
    }

    public Material GetMaterial() => GetMeshRenderer != null ? GetMeshRenderer.sharedMaterial : null;
    public Mesh GetMesh() => GetMeshFilter != null ? GetMeshFilter.sharedMesh : null;
    public Collider GetCollider() => GetTileCollider != null ? GetTileCollider : null;

    public void RotateTile(int direction)
    {
#if UNITY_EDITOR
        Undo.RecordObject(transform, "Rotate Tile");
#endif
        transform.Rotate(0, 90 * direction, 0);

#if UNITY_EDITOR
        EditorUtility.SetDirty(gameObject);
#endif
    }

    public void AdjustVertical(int verticalDirection)
    {
#if UNITY_EDITOR
        Undo.RecordObject(transform, "Adjust Vertical Position");
#endif
        transform.position += new Vector3(0, 0.1f * verticalDirection, 0);

#if UNITY_EDITOR
        EditorUtility.SetDirty(gameObject);
#endif
    }

    public List<GameObject> GetAllChildren()
    {
        List<GameObject> children = new List<GameObject>();

        foreach (Transform child in transform)
        {
            children.Add(child.gameObject);
        }

        return children;
    }

    public void UpdateCollider(Collider newCollider)
    {
        Collider currentCollider = GetTileCollider;
        if (currentCollider != null)
        {
#if UNITY_EDITOR
            Undo.DestroyObjectImmediate(currentCollider);
#else
            DestroyImmediate(currentCollider);
#endif
        }

        if (newCollider is BoxCollider)
        {
            BoxCollider originOne = newCollider.GetComponent<BoxCollider>();
#if UNITY_EDITOR
            BoxCollider newOne = Undo.AddComponent<BoxCollider>(gameObject);
#else
            BoxCollider newOne = gameObject.AddComponent<BoxCollider>();
#endif
            newOne.center = originOne.center;
            newOne.size = originOne.size;
        }

        if (newCollider is MeshCollider)
        {
            MeshCollider originOne = newCollider.GetComponent<MeshCollider>();
#if UNITY_EDITOR
            MeshCollider newOne = Undo.AddComponent<MeshCollider>(gameObject);
#else
            MeshCollider newOne = gameObject.AddComponent<MeshCollider>();
#endif
            newOne.sharedMesh = originOne.sharedMesh;
            newOne.convex = originOne.convex;
        }
    }

    private MeshRenderer GetMeshRenderer => GetComponent<MeshRenderer>();
    private MeshFilter GetMeshFilter => GetComponent<MeshFilter>();
    private Collider GetTileCollider => GetComponent<Collider>();
}
