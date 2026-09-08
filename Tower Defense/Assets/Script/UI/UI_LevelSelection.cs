using UnityEngine;

public class UI_LevelSelection : MonoBehaviour
{
    [SerializeField]
    TileSlot_LevelBtn[] levelBtnTiles;

    private void ActivateButtons(bool canClick)
    {
        foreach (var btn in levelBtnTiles)
        {
            btn.EnableTileClick(canClick);
        }
    }

    private void OnEnable()
    {
        ActivateButtons(true);
    }

    private void OnDisable() 
    {
        ActivateButtons(false);
    }
}
