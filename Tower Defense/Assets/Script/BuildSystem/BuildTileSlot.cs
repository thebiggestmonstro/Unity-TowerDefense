using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class BuildTileSlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler
{
    private Vector3 defaultPosition;
    private bool bCanMove = true;
    private bool bBuildTileAvailable = true;
    private Coroutine currentMoveUpCoroutine;
    private MeshRenderer meshRenderer;
    private Material material;
    private Collider buildTileCollider;
    private UI_Canvas uiCanvas;

    private void Awake()
    {
        defaultPosition = transform.position;
        meshRenderer = GetComponent<MeshRenderer>();
        buildTileCollider = GetComponent<Collider>();
    }

    private void Start()
    {
        uiCanvas = UIManager.GetUI<UI_Canvas>("Canvas");

        if (GetComponent<TileSlot_LevelBtn>() != null)
        {
            bBuildTileAvailable = true;
        }

        if (!bBuildTileAvailable)
        {
            transform.position += new Vector3(0, 0.1f);
            material = meshRenderer.material;
            material.color = Color.red;
            meshRenderer.material = material;
            buildTileCollider.enabled = bBuildTileAvailable;
        }
    }

    private bool CanInteractWithTile(out GridVisibilityController gridVisibility)
    {
        gridVisibility = GameServices.Get<GridVisibilityController>();
        return gridVisibility != null && bBuildTileAvailable && !gridVisibility.GetIsGridMoving();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (!CanInteractWithTile(out _))
        {
            return;
        }

        if (eventData.button != PointerEventData.InputButton.Left)
        {
            return;
        }

        BuildManager buildManager = GameServices.Get<BuildManager>();
        if (buildManager == null || buildManager.GetSelectedBuildTile() == this)
        {
            return;
        }

        buildManager.SetSelectedBuildTile(this);
        buildManager.EnableBuildMenu();

        CancelInvoke(nameof(MoveTileDown));
        MoveTileUp();
        bCanMove = false;

        uiCanvas.uiBuildBtns.GetLastSelectedButton()?.SelectButton(true);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!CanInteractWithTile(out _) || !bCanMove)
        {
            return;
        }

        CancelInvoke(nameof(MoveTileDown));
        MoveTileUp();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (!CanInteractWithTile(out _) || !bCanMove)
        {
            return;
        }

        TileAnimator tileAnimator = GameServices.Get<TileAnimator>();
        if (tileAnimator == null)
        {
            return;
        }

        if (currentMoveUpCoroutine != null)
        {
            Invoke(nameof(MoveTileDown), tileAnimator.GetMovementDurtaion());
        }
        else
        {
            MoveTileDown();
        }
    }

    public void UnSelectTile()
    {
        MoveTileDown();
        bCanMove = true;
    }

    private void MoveTileUp()
    {
        TileAnimator tileAnimator = GameServices.Get<TileAnimator>();
        if (tileAnimator == null)
        {
            return;
        }

        Vector3 targetPosition = transform.position + new Vector3(0, tileAnimator.GetBuildTileOffset(), 0);
        tileAnimator.MoveTile(transform, targetPosition);
        currentMoveUpCoroutine = tileAnimator.GetActiveTileMovementCoroutine(transform);
    }

    private void MoveTileDown()
    {
        GameServices.Get<TileAnimator>()?.MoveTile(transform, defaultPosition);
    }

    public void MoveTileDownImmediate()
    {
        CancelInvoke(nameof(MoveTileDown));
        GameServices.Get<TileAnimator>()?.StopTileMovement(transform);
        transform.position = defaultPosition;
    }

    public Vector3 GetBuildPosition(float yOffset) => defaultPosition + new Vector3(0, yOffset);

    public void SetBuildTileAvailability(bool bValue) => bBuildTileAvailable = bValue;
}