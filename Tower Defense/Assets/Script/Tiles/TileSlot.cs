using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.AI.Navigation;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class TileSlot : MonoBehaviour
{
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
        UpdateNavMeshSurface();
    }
#endif

    public void SwitchTile(GameObject referencedTile)
    {
        TileSlot newTilePrefab = referencedTile.GetComponent<TileSlot>();
        if (newTilePrefab == null)
        {
            return;
        }

#if UNITY_EDITOR
        Undo.IncrementCurrentGroup();
        int undoGroupIndex = Undo.GetCurrentGroup();
        Undo.SetCurrentGroupName("Switch Tile");

        if (GetMeshFilter != null)
        {
            Undo.RecordObject(GetMeshFilter, "Change Mesh");
        }
        if (GetMeshRenderer != null)
        {
            Undo.RecordObject(GetMeshRenderer, "Change Material");
        }
        if (GetTileCollider != null)
        {
            Undo.RecordObject(GetTileCollider, "Change Collider");
        }
        Undo.RegisterCompleteObjectUndo(gameObject, "Change Properties and Name");
        Undo.RecordObject(gameObject, "Change Name and Layer");
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

        UpdateChildren(referencedTile);

        UpdateTileLayer(referencedTile);

        UpdateNavMeshSurface();

        ChangeToBuildTileSlot(referencedTile);

#if UNITY_EDITOR
        Undo.CollapseUndoOperations(undoGroupIndex);
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
        UpdateNavMeshSurface();

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
        UpdateNavMeshSurface();

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

        if (newCollider is BoxCollider originBox)
        {
#if UNITY_EDITOR
            BoxCollider newOne = Undo.AddComponent<BoxCollider>(gameObject);
#else
            BoxCollider newOne = gameObject.AddComponent<BoxCollider>();
#endif
            newOne.center = originBox.center;
            newOne.size = originBox.size;
        }
        else if (newCollider is MeshCollider originMesh)
        {
#if UNITY_EDITOR
            MeshCollider newOne = Undo.AddComponent<MeshCollider>(gameObject);
#else
            MeshCollider newOne = gameObject.AddComponent<MeshCollider>();
#endif
            newOne.sharedMesh = originMesh.sharedMesh;
            newOne.convex = originMesh.convex;
        }
    }

    private void UpdateChildren(GameObject referencedTile)
    {
        List<GameObject> currentChildren = GetAllChildren();
        for (int i = currentChildren.Count - 1; i >= 0; i--)
        {
#if UNITY_EDITOR
            Undo.DestroyObjectImmediate(currentChildren[i]);
#else
            Destroy(currentChildren[i]);
#endif
        }

#if UNITY_EDITOR
        StaticEditorFlags flags = GameObjectUtility.GetStaticEditorFlags(referencedTile);
#endif
        foreach (Transform childTransform in referencedTile.transform)
        {
            GameObject spawnedChild = Instantiate(childTransform.gameObject);
            spawnedChild.transform.position = transform.position;
            spawnedChild.transform.rotation = transform.rotation;

#if UNITY_EDITOR
            Undo.RegisterCreatedObjectUndo(spawnedChild, "Spawn Child Tile");
            Undo.SetTransformParent(spawnedChild.transform, this.transform, "Parent Child Tile");

            GameObjectUtility.SetStaticEditorFlags(spawnedChild, flags);
#else
            spawnedChild.transform.SetParent(this.transform);
#endif
            spawnedChild.transform.localPosition = childTransform.localPosition;
            spawnedChild.transform.localRotation = childTransform.localRotation;
            spawnedChild.transform.localScale = childTransform.localScale;
            spawnedChild.layer = childTransform.gameObject.layer;
        }
    }

    private void UpdateNavMeshSurface()
    {
        if (GetTileNavMeshSurface != null)
        {
            GetTileNavMeshSurface.BuildNavMesh();
        }
    }

    private MeshRenderer GetMeshRenderer => GetComponent<MeshRenderer>();
    private MeshFilter GetMeshFilter => GetComponent<MeshFilter>();
    private Collider GetTileCollider => GetComponent<Collider>();
    private NavMeshSurface GetTileNavMeshSurface => GetComponentInParent<NavMeshSurface>();
    private TileSetHolder GetTileSetHolder => GetComponentInParent<TileSetHolder>();

    private void ChangeToBuildTileSlot(GameObject selectedTile)
    {
        BuildTileSlot buildSlot = GetComponent<BuildTileSlot>();

        if (selectedTile != GetTileSetHolder.tileField)
        {
            if (buildSlot != null)
            {
#if UNITY_EDITOR
                if (!Application.isPlaying)
                {
                    Undo.DestroyObjectImmediate(buildSlot);
                    return;
                }
#endif
                Destroy(buildSlot);
            }
        }
        else
        {
            if (buildSlot == null)
            {
#if UNITY_EDITOR
                if (!Application.isPlaying)
                {
                    buildSlot = Undo.AddComponent<BuildTileSlot>(gameObject);
                }
                else
#endif
                {
                    buildSlot = gameObject.AddComponent<BuildTileSlot>();
                }
            }
        }
    }

    public void UpdateTileLayer(GameObject referencedObject)
    {
        gameObject.layer = referencedObject.layer;
#if UNITY_EDITOR
        StaticEditorFlags flags = GameObjectUtility.GetStaticEditorFlags(referencedObject);
        GameObjectUtility.SetStaticEditorFlags(gameObject, flags);
#endif
    }
}
