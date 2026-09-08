using UnityEngine;
using UnityEngine.EventSystems;

public class TileSlot_LevelBtn : MonoBehaviour, IPointerDownHandler
{
    [SerializeField]
    private int levelIndex;

    private bool canClick;

    public void OnPointerDown(PointerEventData eventData)
    {
        if (!canClick)
        {
            return;
        }

        Debug.Log("I`m Loading Level_" + levelIndex);
    }

    public void EnableTileClick(bool enable) => canClick = enable;
}
