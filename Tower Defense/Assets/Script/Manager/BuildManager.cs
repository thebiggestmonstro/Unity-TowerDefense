using UnityEngine;

public class BuildManager : MonoBehaviour
{
    public static BuildManager Instance { get; private set; }
    public BuildTileSlot selectedBuildTile;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SetSelectedBuildTile(BuildTileSlot newSlot)
    {
        if (selectedBuildTile)
        {
            selectedBuildTile.UnSelectTile();
        }

        selectedBuildTile = newSlot;
    }
}
