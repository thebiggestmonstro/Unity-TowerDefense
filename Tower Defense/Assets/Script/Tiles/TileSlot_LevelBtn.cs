using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class TileSlot_LevelBtn : MonoBehaviour, IPointerDownHandler
{
    [SerializeField]
    private int levelIndex;

    private bool canClick;
    private bool unlocked;

    private void Start()
    {
        CheckLevelUnlocked();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (!canClick || !unlocked)
        {
            Debug.Log("Level Locked!!!");
            return;
        }

        GameEvents.RaiseSceneSelected($"Level_{levelIndex}");
    }

    public void EnableTileClick(bool enable) => canClick = enable;

    public void CheckLevelUnlocked()
    {
        if (levelIndex == 1)
        {
            GameServices.Get<SaveManager>()?.UnlockLevel($"Level_{levelIndex}");
        }

        unlocked = GameServices.Get<SaveManager>()?.IsLevelUnlocked($"Level_{levelIndex}") ?? false;

        if (!unlocked)
        {
            GetComponentInChildren<TextMeshPro>().text = "Locked";
        }
        else
        {
            GetComponentInChildren<TextMeshPro>().text = $"Level_{ levelIndex}";
        }
    }
}
