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

        if (!bBuildTileAvailable)
        {
            transform.position += new Vector3(0, 0.1f);
            material = meshRenderer.material;
            material.color = Color.red;
            meshRenderer.material = material;
            buildTileCollider.enabled = bBuildTileAvailable;
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (!bBuildTileAvailable || GameServices.GridVisibilityController.GetIsGridMoving())
        {
            return;
        }

        if (eventData.button != PointerEventData.InputButton.Left)
        {
            return;
        }

        if (GameServices.BuildManager.GetSelectedBuildTile() == this)
        {
            return;
        }

        GameServices.BuildManager.SetSelectedBuildTile(this);
        GameServices.BuildManager.EnableBuildMenu();

        CancelInvoke(nameof(MoveTileDown));
        MoveTileUp();
        bCanMove = false;

        uiCanvas.uiBuildBtns.GetLastSelectedButton()?.SelectButton(true);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!bBuildTileAvailable || GameServices.GridVisibilityController.GetIsGridMoving())
        {
            return;
        }

        if (!bCanMove)
        {
            return;
        }

        CancelInvoke(nameof(MoveTileDown));
        MoveTileUp();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (!bBuildTileAvailable || GameServices.GridVisibilityController.GetIsGridMoving())
        {
            return;
        }

        if (!bCanMove)
        {
            return;
        }

        if (currentMoveUpCoroutine != null)
        {
            Invoke(nameof(MoveTileDown), GameServices.TileAnimator.GetMovementDurtaion());
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
        Vector3 targetPosition = transform.position + new Vector3(0, GameServices.TileAnimator.GetBuildTileOffset(), 0);
        GameServices.TileAnimator.MoveTile(transform, targetPosition);
        currentMoveUpCoroutine = GameServices.TileAnimator.GetActiveTileMovementCoroutine(transform);
    }

    private void MoveTileDown()
    {
        GameServices.TileAnimator.MoveTile(transform, defaultPosition);
    }

    public void MoveTileDownImmediate()
    {
        CancelInvoke(nameof(MoveTileDown));
        GameServices.TileAnimator.StopTileMovement(transform);
        transform.position = defaultPosition;
    }

    public Vector3 GetBuildPosition(float yOffset) => defaultPosition + new Vector3(0, yOffset);

    public void SetBuildTileAvailability(bool bValue) => bBuildTileAvailable = bValue;
}