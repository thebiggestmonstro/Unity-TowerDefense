using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(TileSlot)), CanEditMultipleObjects]
public class TileSlotEditor : Editor
{
    private TileSetHolder tileSetHolder;

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
            EditorGUILayout.HelpBox("씬에 TileSetHolder 가 존재하지 않습니다.", MessageType.Warning);
            return;
        }

        float buttonWidth = (EditorGUIUtility.currentViewWidth - 25) / 2;

        GUILayout.BeginHorizontal();
        AddTileChangeGUI("Field", buttonWidth, tileSetHolder.tileField);
        AddTileChangeGUI("Road", buttonWidth, tileSetHolder.tileRoad);
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        AddTileChangeGUI("Sideway", buttonWidth * 2, tileSetHolder.tileSideway);
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
            Debug.LogWarning("TileSetHolder에 해당 타일 프리팹이 등록되지 않았습니다.");
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
}
