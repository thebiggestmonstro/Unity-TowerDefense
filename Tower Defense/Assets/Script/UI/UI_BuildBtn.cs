using TMPro;
using UnityEngine;

public class UI_BuildBtn : MonoBehaviour
{
    [SerializeField]
    private GameObject towerToBuild;
    [SerializeField]
    private int costToBuild = 50;
    [SerializeField]
    private string unitName;

    [Space]
    [SerializeField]
    private TextMeshProUGUI unitNameText;
    [SerializeField]
    private TextMeshProUGUI unitCostText;

    [Space]
    [SerializeField]
    private Camera mainCamera;
    [SerializeField]
    private float towerCenterY = 0.5f;

    private CameraEffect camEffect;
    private UI_Canvas uiCanvas;

    private void Awake()
    {
        uiCanvas = GetComponentInParent<UI_Canvas>();
    }

    private void Start()
    {
        camEffect = mainCamera.GetComponent<CameraEffect>();
    }

    public void BuildTower()
    {
        if (towerToBuild == null || !GameManager.Instance.CheckEnoughCurrency(costToBuild))
        {
            UIManager.GetUI<UI_InGame>("UI_InGame").ShakeCurrencyUI();
            return;
        }

        BuildTileSlot selectedTileSlot = BuildManager.Instance.GetSelectedBuildTile();
        BuildManager.Instance.CancleBuildUnit();
        selectedTileSlot.MoveTileDownImmediate();
        selectedTileSlot.SetBuildTileAvailability(false);
        camEffect.Screenshake(0.15f, 0.02f);

        GameObject newTower = Instantiate(towerToBuild, selectedTileSlot.GetBuildPosition(towerCenterY), Quaternion.identity);
    }

    private void OnValidate()
    {
        unitNameText.text = unitName;
        unitCostText.text = costToBuild.ToString();
        gameObject.name = "UI_BuildBtn - " + unitName;
    }

    public void UnlockUnit(string unitNameToUnlock, bool unlockStatus)
    {
        if (unitName != unitNameToUnlock)
        {
            return;
        }

        gameObject.SetActive(unlockStatus);
    }
}