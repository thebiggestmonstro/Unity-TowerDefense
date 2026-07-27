using UnityEngine;

public class UI_BuildBtn : MonoBehaviour
{
    [SerializeField]
    private GameObject towerToBuild;
    [SerializeField]
    private int costToBuild = 50;

    [Space]
    [SerializeField]
    private Camera mainCamera;
    [SerializeField]
    private float towerCenterY = 0.5f;

    private CameraEffect camEffect;

    private void Start()
    {
        camEffect = mainCamera.GetComponent<CameraEffect>();
    }

    public void BuildTower()
    {
        if (towerToBuild == null || !GameManager.Instance.CheckEnoughCurrency(costToBuild))
        {
            return;
        }

        BuildTileSlot selectedTileSlot = BuildManager.Instance.GetSelectedBuildTile();
        BuildManager.Instance.CancleBuildUnit();
        selectedTileSlot.MoveTileDownImmediate();
        selectedTileSlot.SetBuildTileAvailability(false);
        camEffect.Screenshake(0.15f, 0.02f);

        GameObject newTower = Instantiate(towerToBuild, selectedTileSlot.GetBuildPosition(towerCenterY), Quaternion.identity);
    }
}