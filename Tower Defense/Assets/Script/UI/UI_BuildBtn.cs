using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class UI_BuildBtn : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
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
    private VisualEffect_UnitPreview unitPreview;
    public bool bButtonUnlocked { get; private set; }
    private UI_BuildBtnHover onButtonHoverEffect;
    private UI_BuildBtns buildButtonsHolder;

    private void Awake()
    {
        uiCanvas = GetComponentInParent<UI_Canvas>();
        camEffect = mainCamera.GetComponent<CameraEffect>();
        onButtonHoverEffect = GetComponent<UI_BuildBtnHover>();
        buildButtonsHolder = GetComponentInParent<UI_BuildBtns>();
    }

    private void Start()
    {
        CreateUnitPreview();
    }

    public void BuildTower()
    {
        if (towerToBuild == null || !GameManager.Instance.CheckEnoughCurrency(costToBuild))
        {
            UIManager.GetUI<UI_InGame>("UI_InGame").ShakeCurrencyUI();
            return;
        }

        if (uiCanvas.uiBuildBtns.GetLastSelectedButton() == null)
        {
            return;
        }

        BuildTileSlot selectedTileSlot = BuildManager.Instance.GetSelectedBuildTile();
        BuildManager.Instance.CancleBuildUnit();
        selectedTileSlot.MoveTileDownImmediate();
        selectedTileSlot.SetBuildTileAvailability(false);
        uiCanvas.uiBuildBtns.SetLastSelected(null);
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

        bButtonUnlocked = unlockStatus;
        gameObject.SetActive(unlockStatus);
    }

    private void CreateUnitPreview()
    {
        GameObject newPreview = Instantiate(towerToBuild, Vector3.zero, Quaternion.identity);
        unitPreview = newPreview.AddComponent<VisualEffect_UnitPreview>();
        unitPreview.gameObject.SetActive(false);
    }

    public void SelectButton(bool bIsSelected)
    {
        if (unitPreview == null)
        {
            if (bIsSelected)
            {
                CreateUnitPreview(); 
            }

            if (unitPreview == null)
            {
                return;
            }
        }

        BuildTileSlot slotToUse = BuildManager.Instance.GetSelectedBuildTile();

        if (slotToUse == null)
        {
            return;
        }

        Vector3 previewPosition = slotToUse.GetBuildPosition(1);
        unitPreview.gameObject.SetActive(bIsSelected);
        unitPreview.ShowPreview(bIsSelected, previewPosition);
        onButtonHoverEffect.ShowButton(bIsSelected);
        buildButtonsHolder.SetLastSelected(this);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        BuildManager.Instance.SetMouseOnUI(true);

        foreach (var button in buildButtonsHolder.GetBuildButtons())
        {
            if (button.gameObject.activeSelf)
            {
                button.SelectButton(false);
            }
        }

        SelectButton(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        BuildManager.Instance.SetMouseOnUI(false);
    }
}