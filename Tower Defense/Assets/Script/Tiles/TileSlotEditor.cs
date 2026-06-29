using UnityEditor;
using UnityEngine;

#if UNITY_EDITOR

[CustomEditor(typeof(TileSlot)), CanEditMultipleObjects]
public class TileSlotEditor : Editor
{
    private TileSetHolder tileSetHolder;
    private GUIStyle centeredGUIStyle;

    private void OnEnable()
    {
        tileSetHolder = FindFirstObjectByType<TileSetHolder>();
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        base.OnInspectorGUI();

        if (tileSetHolder == null)
        {
            return;
        }

        centeredGUIStyle = new GUIStyle(GUI.skin.label)
        {
            alignment = TextAnchor.MiddleCenter,
            fontStyle = FontStyle.Bold,
            fontSize = 16
        };

        float oneBtnWidth = (EditorGUIUtility.currentViewWidth - 25);
        float twoBtnWidth = (EditorGUIUtility.currentViewWidth - 25) / 2;
        float threeBtnWidth = (EditorGUIUtility.currentViewWidth - 25) / 3;

        GUILayout.Label("Tile Position and Rotation", centeredGUIStyle);

        GUILayout.BeginHorizontal();
        AddTileTRotationGUI("Rotate Left", twoBtnWidth, tileSetHolder.tileField, -1);
        AddTileTRotationGUI("Rotate Right", twoBtnWidth, tileSetHolder.tileField, 1);
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        AddTileVerticalGUI("-0.1f on the Y", twoBtnWidth, tileSetHolder.tileField, -1);
        AddTileVerticalGUI("+0.1f on the Y", twoBtnWidth, tileSetHolder.tileField, +1);
        GUILayout.EndHorizontal();

        GUILayout.Label("Tile Options", centeredGUIStyle);

        GUILayout.BeginHorizontal();
        AddTileChangeGUI("Field", twoBtnWidth, tileSetHolder.tileField);
        AddTileChangeGUI("Road", twoBtnWidth, tileSetHolder.tileRoad);
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        AddTileChangeGUI("Sideway", oneBtnWidth, tileSetHolder.tileSideway);
        GUILayout.EndHorizontal();

        GUILayout.Label("Corner Options", centeredGUIStyle);

        GUILayout.BeginHorizontal();
        AddTileChangeGUI("Inner Corner", twoBtnWidth, tileSetHolder.tileInnerCorner);
        AddTileChangeGUI("Outer Corner", twoBtnWidth, tileSetHolder.tileOuterCorner);
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        AddTileChangeGUI("Inner Corner Small", twoBtnWidth, tileSetHolder.tileInnerCornerSmall);
        AddTileChangeGUI("Outer Corner Small", twoBtnWidth, tileSetHolder.tileOuterCornerSmall);
        GUILayout.EndHorizontal();

        GUILayout.Label("Bridges and Hills", centeredGUIStyle);

        GUILayout.BeginHorizontal();
        AddTileChangeGUI("Hill 1", threeBtnWidth, tileSetHolder.tileHill_1);
        AddTileChangeGUI("Hill 2", threeBtnWidth, tileSetHolder.tileHill_2);
        AddTileChangeGUI("Hill 3", threeBtnWidth, tileSetHolder.tileHill_3);
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        AddTileChangeGUI("Bridge of Field", threeBtnWidth, tileSetHolder.tileBridgeField);
        AddTileChangeGUI("Bridge of Road", threeBtnWidth, tileSetHolder.tileBridgeRoad);
        AddTileChangeGUI("Bridge of Sideway", threeBtnWidth, tileSetHolder.tileBridgeSideway);
        GUILayout.EndHorizontal();
    }

    private void AddTileChangeGUI(string GUIName, float GUIWidth, GameObject targetPrefab)
    {
        if (GUILayout.Button(GUIName, GUILayout.Width(GUIWidth)))
        {
            ApplyTileChange(targetPrefab);
        }
    }

    private void ApplyTileChange(GameObject targetPrefab)
    {
        if (targetPrefab == null)
        {
            return;
        }

        foreach (Object targetTile in targets)
        {
            if (targetTile is TileSlot slot)
            {
                slot.SwitchTile(targetPrefab);
            }
        }
    }

    private void AddTileTRotationGUI(string GUIName, float GUIWidth, GameObject targetPrefab, int rotationValue)
    {
        if (GUILayout.Button(GUIName, GUILayout.Width(GUIWidth)))
        {
            ApplyRotationChange(targetPrefab, rotationValue);
        }
    }

    private void ApplyRotationChange(GameObject targetPrefab, int rotationValue)
    {
        if (targetPrefab == null)
        {
            return;
        }

        foreach (Object targetTile in targets)
        {
            if (targetTile is TileSlot slot)
            {
                slot.RotateTile(rotationValue);
            }
        }
    }

    private void AddTileVerticalGUI(string GUIName, float GUIWidth, GameObject targetPrefab, int verticalValue)
    {
        if (GUILayout.Button(GUIName, GUILayout.Width(GUIWidth)))
        {
            ApplyVerticalChange(targetPrefab, verticalValue);
        }
    }

    private void ApplyVerticalChange(GameObject targetPrefab, int verticalValue)
    {
        if (targetPrefab == null)
        {
            return;
        }

        foreach (Object targetTile in targets)
        {
            if (targetTile is TileSlot slot)
            {
                slot.AdjustVertical(verticalValue);
            }
        }
    }
}

#endif