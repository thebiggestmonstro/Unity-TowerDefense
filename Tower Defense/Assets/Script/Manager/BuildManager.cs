using UnityEngine;
using UnityEngine.InputSystem;

public class BuildManager : MonoBehaviour
{
    [SerializeField]
    private UI_Canvas uiCanvas;
    [SerializeField]
    private Camera mainCamera;

    public static BuildManager Instance { get; private set; }
    private BuildTileSlot selectedBuildTile;

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

    private void Update()
    {
        if (Keyboard.current[Key.Escape].wasPressedThisFrame)
        {
            CancleBuildUnit();
        }

        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            if (Physics.Raycast(mainCamera.ScreenPointToRay(Mouse.current.position.ReadValue()), out RaycastHit hit))
            {
                if (!hit.collider.TryGetComponent<BuildTileSlot>(out _))
                {
                    CancleBuildUnit();
                }
            }
        }
    }

    public void CancleBuildUnit()
    {
        if (selectedBuildTile == null)
        { 
            return; 
        }
        
        selectedBuildTile.UnSelectTile();
        selectedBuildTile = null;
        DisableBuildMenu();
    }

    public void SetSelectedBuildTile(BuildTileSlot newSlot)
    {
        if (selectedBuildTile)
        {
            selectedBuildTile.UnSelectTile();
        }

        selectedBuildTile = newSlot;
    }

    public void EnableBuildMenu()
    {
        if (selectedBuildTile == null)
        {
            return;
        }

        uiCanvas.uiBuildBtns.ShowBuildButtons(true);
    }

    private void DisableBuildMenu()
    {
        uiCanvas.uiBuildBtns.ShowBuildButtons(false);
    }

    public BuildTileSlot GetSelectedBuildTile() => selectedBuildTile;
}
